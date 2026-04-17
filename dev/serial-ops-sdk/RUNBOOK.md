# Serial Ops Runbook

Minimal repeatable workflow for confirming SIC serial operations on Linux.

## 1) Build/check host tools in sibling SDK repo

```bash
cd ../sdk
make tools-native
```

## 2) Map COM1 to your USB serial device

```bash
export SIC_COM1=/dev/ttyUSB0
```

If needed, confirm permissions:

```bash
ls -l "$SIC_COM1"
id
```

## 3) Handshake/ops confirmation (quick mode)

From this repo root:

```bash
./dev/serial-ops-sdk/run-betix-ft323.sh
```

Or explicit validator calls:

```bash
./dev/serial-ops-sdk/validate-link.sh --device /dev/ttyUSB0 --path FT323
```

or:

```bash
./dev/serial-ops-sdk/validate-link.sh --device /dev/ttyUSB0 --path MAX3232
```

Quick mode uses a timeout and treats successful CROM handshake (`#CROM11#` sequence path) as pass.

Betix convenience runner:

```bash
./dev/serial-ops-sdk/run-betix-ft323.sh /dev/ttyUSB1
```

## 4) Full readback confirmation (optional)

```bash
./dev/serial-ops-sdk/validate-link.sh --device /dev/ttyUSB0 --path FT323 --full
```

`--full` runs a complete `sicgrab` dump (256K) instead of timeout-limited probe mode.

## 5) Where results go

Run artifacts are written to:

```text
dev/serial-ops-sdk/runs/<run-id>/
```

Each run directory contains:

* `sicgrab.log` - raw command output
* `probe.ic3` - dump output in full mode (or partial file in quick mode)
* `summary.txt` - compact pass/fail summary
