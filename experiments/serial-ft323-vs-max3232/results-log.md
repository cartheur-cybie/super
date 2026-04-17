# Results Log (SIC Serial Comparative)

Record one row per run.

| Run ID | Date | SIC Unit | Path | Adapter/Board | Cable/Jack Map | Host + Terminal | Voltage Gate | `U` Stream | Keypress Stops `U` | CROM Prompt | 30m Stable | Fan Behavior | Errors | Valid Run |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---:|---|
| 20260417-01 | 2026-04-17 | Betix | FT323 | TBD | Tx-tip / Rx-center / GND-base | TBD | TBD | TBD | TBD | TBD | TBD | TBD | TBD | TBD |

## Photo Capture Per Run

Capture and store these with each run (relative path suggestion: `experiments/serial-ft323-vs-max3232/photos/<run-id>/`):

* SIC-side connector/wiring before power-on.
* Adapter board and label (to confirm exact model used).
* Meter reading for idle TX voltage.
* Host terminal screenshot showing `U` stream and CROM prompt transition.

## Detailed Notes (append per run)

### Run ID: YYYYMMDD-##

* SIC unit:
* Path (`FT323` or `MAX3232`):
* Exact adapter model:
* Exact cable/jack/DB9 mapping used:
* Measured idle TX values:
* Handshake observations (`U` stream + keypress response):
* Fan behavior timeline:
* Any garbled text/timeouts:
* Reconnect result:
* Verdict:

## Start Here: Betix FT323 First Run Template

Run this first:

```bash
./dev/serial-ops-sdk/run-betix-ft323.sh
```

Alternate explicit command:

```bash
./dev/serial-ops-sdk/validate-link.sh --device /dev/ttyUSB0 --path FT323
```

Then copy/fill this block:

### Run ID: YYYYMMDD-FT323-BETIX-01

* SIC unit: Betix
* Path: FT323
* CROMINST status: preinstalled
* Host device: /dev/ttyUSB0
* Exact adapter model: DFRobot FT323/FT232 breakout
* Cable mapping: TXD->ring(center), RXD->tip, GND->sleeve(base)
* Measured idle TX voltage:
* `U` stream observed (Y/N):
* Keypress stopped `U` (Y/N):
* CROM prompt observed (Y/N):
* Fan behavior timeline:
* Serial errors/garble:
* Reconnect result:
* Verdict (PASS/FAIL):
