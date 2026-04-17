#!/usr/bin/env bash
set -euo pipefail

# Local convenience runner for first live FT323 test on Betix.
# Usage:
#   ./dev/serial-ops-sdk/run-betix-ft323.sh
#   ./dev/serial-ops-sdk/run-betix-ft323.sh /dev/ttyUSB1

DEVICE="${1:-/dev/ttyUSB0}"
ROOT_DIR="$(git rev-parse --show-toplevel)"
VALIDATOR="$ROOT_DIR/dev/serial-ops-sdk/validate-link.sh"

if [[ ! -x "$VALIDATOR" ]]; then
  echo "ERROR: validator not found/executable: $VALIDATOR" >&2
  exit 1
fi

echo "Running Betix FT323 quick validation"
echo "Device: $DEVICE"
echo
"$VALIDATOR" --device "$DEVICE" --path FT323

echo
echo "Next: record this run in"
echo "  $ROOT_DIR/experiments/serial-ft323-vs-max3232/results-log.md"
