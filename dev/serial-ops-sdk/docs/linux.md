# Linux Workflow (Minimal Wine)

This SDK now uses a hybrid approach on Linux:

- Native Linux binaries for open-source tools in `binsrc/` (`sicburn`, `sicgrab`, `convic3`, `splitic3`, `mergeic3`, `adp2c`).
- Wine only for closed-source Toshiba/MS toolchain binaries (`CC900.EXE`, `ASM900.EXE`, `TULINK.EXE`, `TUCONV.EXE`, `TULIB.EXE`, `NMAKE.EXE`, `THC1.EXE`, `THC2.EXE`, `adpcm66.exe`).

## Prerequisites

Install:

- `wine`
- `make`
- `g++`

Then validate your environment:

```bash
make doctor
```

## Quick Start

1. Build native host tools and copy them into `bin/`:

```bash
make tools-native
```

2. Build runtime library (`lib/iclib2.lib`, `lib/crt3.o`) with Wine-backed toolchain:

```bash
make lib
```

3. Build a sample:

```bash
make sample SAMPLE=hello
```

Artifacts are generated in the sample directory as with the legacy flow (`*-l.bin`, `*-h.bin`).

## Flashing on Linux (Native `sicburn`)

Use native `sicburn` and map COM port numbers through environment variables:

```bash
export SIC_COM1=/dev/ttyUSB0
./bin/sicburn -1 samples/hello/hello
```

Notes:

- `-1`, `-2`, `-3`, `-4` still select COM1..COM4 as before.
- On Linux, COM numbers resolve via `SIC_COM1`, `SIC_COM2`, etc.
- If `SIC_COM<n>` is not set, default is `/dev/ttyS<n-1>`.
- Ensure your user has serial-port permissions (for example `dialout` group on Debian/Ubuntu).

To grab ROM/cart images natively:

```bash
export SIC_COM1=/dev/ttyUSB0
./bin/sicgrab -1 dump.ic3
```

## Wine COM Mapping (Fallback)

If you need to run the legacy Windows `sicburn.exe` under Wine, map Linux serial devices into Wine COM devices:

```bash
./scripts/linux/map-com.sh /dev/ttyUSB0 1
```

## Useful Commands

```bash
make clean SAMPLE=hello
./scripts/linux/build-sample.sh test1
./scripts/linux/win-tool.sh CC900.EXE
```

## Design Goal

Linux is the primary developer experience. Wine is intentionally limited to vendor-provided binaries that currently have no source replacement in this repository.

## Known Blocker (Current Branch)

On this branch, native host tooling is working on Linux, but compiling with `CC900.EXE` under Wine can still fail on modern Wine builds.

Observed failure pattern:

```text
THC1-Warning-501: Illegal option 'WINEDATADIR=...'
...
ASM900-Fatal-100 : No source file found in invocation
```

What this means:

- The Linux-native tools (`sicburn`, `sicgrab`, `convic3`, `splitic3`, `mergeic3`, `adp2c`) are operational.
- The proprietary Toshiba compile/link chain is still compatibility-sensitive under Wine and may not be reliable on all Wine versions.

Workaround:

- Use Linux-native tools for transfer/packaging flows.
- Run `CC900/ASM900/TULINK` in a known-good Windows environment (VM/host) until a stable Wine recipe is identified.

Related runtime warning:

```text
error: XDG_RUNTIME_DIR is invalid or not set in the environment.
```

Set it before Wine runs:

```bash
export XDG_RUNTIME_DIR=/tmp/xdg-$UID
mkdir -p "$XDG_RUNTIME_DIR"
chmod 700 "$XDG_RUNTIME_DIR"
```
