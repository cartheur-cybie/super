# EM1016 Serial Scope Test (.NET)

Small console tool to validate serial behavior over the Eminent EM1016 and correlate TX/RX activity with oscilloscope captures.

## What it does

- Lists serial ports (`--list`)
- Opens a selected port with configurable UART settings
- Sends deterministic TX patterns for scope probing
- Logs RX bytes with timestamps in hex + ASCII

## Build

```bash
cd experiments/em1016-serial-scope-test
DOTNET_CLI_HOME=$PWD/../../.dotnet-home dotnet build
```

## Quick start

List ports:

```bash
cd experiments/em1016-serial-scope-test
DOTNET_CLI_HOME=$PWD/../../.dotnet-home dotnet run -- --list
```

Transmit alternating pattern for scope (`0x55 0xAA` burst):

```bash
DOTNET_CLI_HOME=$PWD/../../.dotnet-home dotnet run -- \
  --port /dev/ttyUSB0 \
  --mode tx \
  --pattern alt55aa \
  --burst-length 32 \
  --interval-ms 100 \
  --repeat 200
```

TX + RX combined test:

```bash
DOTNET_CLI_HOME=$PWD/../../.dotnet-home dotnet run -- \
  --port /dev/ttyUSB0 \
  --baud 115200 \
  --mode txrx \
  --pattern text
```

Custom hex payload:

```bash
DOTNET_CLI_HOME=$PWD/../../.dotnet-home dotnet run -- \
  --port /dev/ttyUSB0 \
  --mode tx \
  --pattern hex \
  --hex "DE AD BE EF" \
  --repeat 20
```

## Scope tips

- Clip probe ground to serial ground first.
- Probe TX line and trigger on falling edge.
- `alt55aa` helps visualize clean bit timing and polarity.
- For RS-232 lines, expect bipolar voltage swing; for TTL lines, expect 0 to VCC.

## Notes

- Hardware flow control is disabled by default.
- Stop bits currently support `1` or `2`.
- Press `Ctrl+C` to stop infinite runs (`--repeat 0`).
