#!/usr/bin/env bash
set -euo pipefail

# Sync the serial-ops extract from the sibling SDK repo.
# Usage:
#   ./sync-from-sdk.sh
#   SDK_ROOT=/abs/path/to/sdk ./sync-from-sdk.sh

ROOT_DIR="$(git rev-parse --show-toplevel)"
DEST_DIR="$ROOT_DIR/dev/serial-ops-sdk"
SDK_ROOT="${SDK_ROOT:-$ROOT_DIR/../sdk}"

if [[ ! -d "$SDK_ROOT/.git" ]]; then
  echo "ERROR: SDK repo not found at: $SDK_ROOT" >&2
  exit 1
fi

copy_file() {
  local src_rel="$1"
  local src="$SDK_ROOT/$src_rel"
  local dst="$DEST_DIR/$src_rel"
  if [[ ! -f "$src" ]]; then
    echo "ERROR: missing source file: $src" >&2
    exit 1
  fi
  mkdir -p "$(dirname "$dst")"
  cp "$src" "$dst"
  echo "synced: $src_rel"
}

copy_file "binsrc/siclib.cpp"
copy_file "binsrc/siclib.h"
copy_file "binsrc/sicburn.cpp"
copy_file "binsrc/sicgrab.cpp"
copy_file "binsrc/std.h"
copy_file "docs/linux.md"
copy_file "inc/ic_serial.h"
copy_file "libsrc/serial.c"
copy_file "libsrc/main0.c"
copy_file "samples/clinic/clinic.c"

PIN_HASH="$(git -C "$SDK_ROOT" rev-parse HEAD)"
echo
echo "Sync complete."
echo "Upstream SDK commit: $PIN_HASH"
echo "Update dev/serial-ops-sdk/README.md 'Upstream Pin' if you intend to pin to this commit."
