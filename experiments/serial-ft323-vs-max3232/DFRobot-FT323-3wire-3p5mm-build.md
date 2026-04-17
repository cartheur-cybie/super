# DFRobot FT323 Breakout + 3-Wire 3.5mm iCybie Connector (No-Solder)

Use this page for the direct TTL path with a ready-to-go DFRobot FT323/FT232-style breakout and a 3-wire 3.5mm connector for SIC serial.

## Preconditions

This page assumes the internal Super iCybie serial upgrade soldering has already been done per the project docs.
No extra iCybie soldering is covered here.
It also assumes CROMINST is already burned/installed on the SIC.

## Hardware Style for This Build

Use plug-in jumpers on the breakout's bottom header pins.
Skip side-wire soldering on the breakout for this workflow.

## Wiring Summary (Use This First)

| FT323 Breakout Pin | Connect To | Notes |
|---|---|---|
| `TXD` | iCybie `RX` (center/ring) | cross TX->RX |
| `RXD` | iCybie `TX` (tip) | cross RX<-TX |
| `GND` | iCybie `GND` (base/sleeve) | common ground |

Do not connect breakout `VCC` to iCybie serial lines.

## Breakout -> 3-Wire Cable -> Stereo Connector Table

| Breakout Pin | 3-Wire Cable Label | Stereo Connector Conductor | SIC Signal |
|---|---|---|---|
| `TXD` | `RX wire` | `ring` (center) | `SIC RX` |
| `RXD` | `TX wire` | `tip` | `SIC TX` |
| `GND` | `GND wire` | `sleeve` (base) | `SIC GND` |

## 3.5mm 3-Wire Connector Mapping (Repo Standard)

| 3.5mm conductor | iCybie signal |
|---|---|
| tip | `TX` |
| center/ring | `RX` |
| base/sleeve | `GND` |

## Parts (No Solder Build)

| Item | Check |
|---|---|
| DFRobot FT323/FT232 breakout (preassembled) | ☐ |
| 3-wire 3.5mm iCybie cable/adapter | ☐ |
| Female-female jumpers for bottom header pins | ☐ |
| Meter (continuity + voltage) | ☐ |

## Setup Steps (No Solder)

| Step | Check |
|---|---|
| Identify bottom-header FT323 pins: `TXD`, `RXD`, `GND` | ☐ |
| Identify iCybie cable conductors: `tip`, `ring`, `sleeve` | ☐ |
| Plug jumpers on bottom header: `TXD -> ring(center)`, `RXD -> tip`, `GND -> sleeve(base)` | ☐ |
| Secure wires so they do not slip during testing | ☐ |
| Verify connector orientation before powering SIC | ☐ |

## Preflight Electrical Checks (Before SIC Connection)

| Check | Pass condition |
|---|---|
| Continuity | End-to-end continuity for each signal |
| Isolation | No short between any two conductors |
| Cross mapping | FT323 `TXD -> SIC RX`, FT323 `RXD -> SIC TX` |
| Ground | FT323 ground continuous to SIC ground |
| TTL level | SIC-facing high in safe 3.3V domain (not ~5V) |

## First Bring-Up

1. Connect FT323 cable to SIC serial jack.
2. Connect FT323 USB to host.
3. Run quick validation:

```bash
./dev/serial-ops-sdk/validate-link.sh --device /dev/ttyUSB0 --path FT323
```

4. Confirm expected behavior:
   - CROM handshake path succeeds.
   - No abnormal fan behavior.
   - No serial garbage/timeouts beyond occasional recoverable noise.

## Troubleshooting

| Symptom | Likely cause | Fix |
|---|---|---|
| No handshake | TX/RX reversed | swap FT323 `TXD` and `RXD` lines |
| Intermittent connect | loose jumper/contact | reseat jumpers and secure cable |
| Garbled data | wrong device/baud path | verify correct device and run standard tools |
| SIC instability/fan anomaly | electrical noise/level issue | stop test, re-check voltage and grounding |

## No-CROMINST Recovery Path

If CROMINST is not installed on SIC, the FT323 path might show serial activity but still fail the expected ops handshake flow.

Recovery steps:

1. Boot SIC with a CROMINST-capable cartridge/setup.
2. Use terminal at `9600 8N1` and verify repeating `U` stream.
3. Press a key to stop `U` stream and enter CROMINST menu.
4. Run the install flow (`I`, then `YES`) and wait for completion.
5. Reboot SIC and rerun FT323 quick validation:

```bash
./dev/serial-ops-sdk/validate-link.sh --device /dev/ttyUSB0 --path FT323
```

## Photo References

| Reference | Image |
|---|---|
| Actual FT232RL breakout used for this path | <img src="../../images/dfrobot-ftdi" alt="DFRobot FT323/FT232RL breakout board" width="260"> |

### Connection Workflow (Between Photos)

| Breakout Pin | 3-Wire Cable Label | Stereo Connector Conductor | SIC Signal |
|---|---|---|---|
| `TXD` | `RX wire` | `ring` (center) | `SIC RX` |
| `RXD` | `TX wire` | `tip` | `SIC TX` |
| `GND` | `GND wire` | `sleeve` (base) | `SIC GND` |

| Reference | Image |
|---|---|
| TTL cable orientation reference | <img src="../../images/cablecyble.jpg" alt="TTL cable orientation" width="260"> |
| SIC serial connector area | <img src="../../images/serialconnect.jpg" alt="SIC serial connector area" width="260"> |
