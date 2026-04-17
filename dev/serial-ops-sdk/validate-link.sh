#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'EOF'
Usage:
  validate-link.sh --device /dev/ttyUSB0 --path FT323|MAX3232 [--full]
Options:
  --device   Linux serial device to map to COM1 (required)
  --path     Adapter path label for logging (required)
  --full     Run full sicgrab (no timeout). Default is quick probe mode.
EOF
}

DEVICE=""
PATH_LABEL=""
FULL_MODE=0

while [[ $# -gt 0 ]]; do
  case "$1" in
    --device)
      DEVICE="${2:-}"
      shift 2
      ;;
    --path)
      PATH_LABEL="${2:-}"
      shift 2
      ;;
    --full)
      FULL_MODE=1
      shift
      ;;
    -h|--help)
      usage
      exit 0
      ;;
    *)
      echo "Unknown argument: $1" >&2
      usage
      exit 1
      ;;
  esac
done

if [[ -z "$DEVICE" || -z "$PATH_LABEL" ]]; then
  usage
  exit 1
fi

if [[ "$PATH_LABEL" != "FT323" && "$PATH_LABEL" != "MAX3232" ]]; then
  echo "ERROR: --path must be FT323 or MAX3232" >&2
  exit 1
fi

if [[ ! -e "$DEVICE" ]]; then
  echo "ERROR: device does not exist: $DEVICE" >&2
  exit 1
fi

ROOT_DIR="$(git rev-parse --show-toplevel)"
SDK_ROOT="${SDK_ROOT:-$ROOT_DIR/../sdk}"
SICGRAB_BIN="${SICGRAB_BIN:-$SDK_ROOT/bin/sicgrab}"

if [[ ! -x "$SICGRAB_BIN" ]]; then
  echo "ERROR: sicgrab binary not found/executable at: $SICGRAB_BIN" >&2
  echo "Hint: build it in ../sdk via: make -C ../sdk tools-native" >&2
  exit 1
fi

RUN_ID="$(date +%Y%m%d-%H%M%S)-${PATH_LABEL,,}"
RUN_DIR="$ROOT_DIR/dev/serial-ops-sdk/runs/$RUN_ID"
mkdir -p "$RUN_DIR"

LOG_FILE="$RUN_DIR/sicgrab.log"
OUT_FILE="$RUN_DIR/probe.ic3"
SUMMARY_FILE="$RUN_DIR/summary.txt"

export SIC_COM1="$DEVICE"

echo "Run ID: $RUN_ID"
echo "Path: $PATH_LABEL"
echo "Device: $DEVICE"
echo "sicgrab: $SICGRAB_BIN"
echo "Mode: $([[ $FULL_MODE -eq 1 ]] && echo full || echo quick)"
echo
echo "Reset/power-cycle SIC now if needed, then starting probe..."

set +e
if [[ $FULL_MODE -eq 1 ]]; then
  "$SICGRAB_BIN" -1 "$OUT_FILE" 2>&1 | tee "$LOG_FILE"
  RC=$?
else
  timeout 30s "$SICGRAB_BIN" -1 "$OUT_FILE" 2>&1 | tee "$LOG_FILE"
  RC=$?
fi
set -e

HANDSHAKE_OK=0
if rg -q "Setting baud rate to 38.4kbps|Connected!" "$LOG_FILE"; then
  HANDSHAKE_OK=1
fi

SERIAL_WARN=0
if rg -q "CROM WARNING: serial" "$LOG_FILE"; then
  SERIAL_WARN=1
fi

STATUS="FAIL"
if [[ $FULL_MODE -eq 1 && $RC -eq 0 && $HANDSHAKE_OK -eq 1 ]]; then
  STATUS="PASS"
elif [[ $FULL_MODE -eq 0 && $HANDSHAKE_OK -eq 1 ]]; then
  STATUS="PASS"
fi

{
  echo "run_id=$RUN_ID"
  echo "path=$PATH_LABEL"
  echo "device=$DEVICE"
  echo "mode=$([[ $FULL_MODE -eq 1 ]] && echo full || echo quick)"
  echo "exit_code=$RC"
  echo "handshake_ok=$HANDSHAKE_OK"
  echo "serial_warning=$SERIAL_WARN"
  echo "status=$STATUS"
  echo "log_file=$LOG_FILE"
  echo "out_file=$OUT_FILE"
} > "$SUMMARY_FILE"

echo
echo "Summary: $STATUS"
echo "Handshake: $HANDSHAKE_OK"
echo "Serial warnings: $SERIAL_WARN"
echo "Artifacts: $RUN_DIR"

if [[ "$STATUS" != "PASS" ]]; then
  exit 1
fi
