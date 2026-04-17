# Serial Ops SDK Extract

Local copy of relevant serial-operation code from sibling repo `../sdk`.

Purpose:

- Keep the core serial connect/handshake and ops-validation code in this repo for focused FT323 vs MAX3232 development work.

Copied sources:

- `binsrc/siclib.cpp`, `binsrc/siclib.h`, `binsrc/std.h`
- `binsrc/sicburn.cpp`, `binsrc/sicgrab.cpp`
- `docs/linux.md`
- `inc/ic_serial.h`
- `libsrc/serial.c`, `libsrc/main0.c`
- `samples/clinic/clinic.c`

Local tooling added in this repo:

- `validate-link.sh` - quick/full FT323 or MAX3232 serial link validator
- `run-betix-ft323.sh` - one-command local runner for Betix FT323 quick test
- `sync-from-sdk.sh` - refresh copied SDK files from sibling `../sdk`

## Description

| File | Role |
|---|---|
| `binsrc/siclib.cpp` | Host-side serial transport and CROM handshake (`#CROM11#`, `[UPL]`), error count, read/write primitives |
| `binsrc/siclib.h` | Public interface for host-side SIC serial operations |
| `binsrc/std.h` | Shared host-tool typedefs/macros used by `siclib/sicburn/sicgrab` |
| `binsrc/sicburn.cpp` | Host uploader/writer flow for SIC ROM/cartridge over serial |
| `binsrc/sicgrab.cpp` | Host reader/dumper flow to confirm link and read back ROM/cartridge data |
| `docs/linux.md` | Linux COM mapping and command workflow (`SIC_COM1`, native `sicburn/sicgrab`) |
| `inc/ic_serial.h` | Firmware-side serial API contract (`sic_init_serial`, `sic_get_serial_byte`) |
| `libsrc/serial.c` | Firmware UART implementation and RX/TX interrupt handlers |
| `libsrc/main0.c` | Runtime startup enabling serial and printing initial banner |
| `samples/clinic/clinic.c` | Interactive firmware sample useful for live serial ops checks |

Upstream location:

- `/home/cartheur/ame/aiventure/aiventure-github/i-cybie/sdk`

## Upstream Pin

This extract is pinned to SDK commit:

* `6fca4e4e9fd53f8266ffa0dbec4f82a1c625bfaf`

Use `./sync-from-sdk.sh` to refresh this folder from the sibling SDK repo, then update this pinned hash.
