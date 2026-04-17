# Test Protocol (FT323 vs MAX3232)

## 0) Controls (hold constant)

| Control | Requirement |
|---|---|
| SIC unit | Same SIC for A/B pair (start with Betix if CROM-confirmed) |
| Host | Same host machine, terminal app, and USB port where possible |
| Terminal config | `9600 baud`, `8 data bits`, `no parity`, `no handshake` |
| Power setup | Same power arrangement per pair; do not change mid-comparison |

## 1) Preflight Safety Gate (mandatory)

### 1.1 Photo checkpoints

| Checkpoint | Reference |
|---|---|
| Internal layout | `../../images/connections.jpg` |
| Serial area close-up | `../../images/serialconnect.jpg` |
| RS-232 pinout | `../../rs232-ftdi/DB9-Pinout.jpg` |

### 1.2 Wiring + voltage checks

| Check | Pass condition |
|---|---|
| Continuity | End-to-end continuity for each conductor |
| Isolation | No shorts between signal and ground |
| SIC mapping | `Tx -> tip`, `Rx -> center`, `GND -> base` (or explicitly documented variant) |
| TTL voltage gate | SIC-facing logic level in safe 3.3V domain |
| RS-232 idle check | DB9 TX/RX idle shows expected negative RS-232 voltage |

## 2) Path Setup

### 2.1 Path A: FT323/FT232R direct TTL

| Item | Requirement |
|---|---|
| Signals used | `TX`, `RX`, `GND` only (no VCC feed to SIC serial header) |
| Cross wiring | `adapter TX -> SIC RX`, `adapter RX -> SIC TX` |
| EEPROM profile | Record if `drivers/iCybie-Serial.xml` profile is applied |
| Photos | `../../images/FT232RL.png`, `../../images/cablecyble.jpg` |

### 2.2 Path B: MAX3232 RS-232 chain

| Item | Requirement |
|---|---|
| Translator | MAX3232 path only (no MAX233A in test path) |
| DB9 mapping | `ground -> pin 5`, `tip -> pin 2` (iCybie -> PC), `ring/center -> pin 3` (PC -> iCybie) |
| Photos | `../../images/rs232.jpg`, `../../rs232-ftdi/DB9-Pinout.jpg` |

## 3) SIC Handshake Test (primary comparator)

| Step | Expected result |
|---|---|
| Insert CROMINST-capable cartridge/setup | SIC boot context ready |
| Connect test path and power SIC | Link initializes |
| Observe terminal | Repeating `U` stream appears |
| Press any key | `U` stream stops |
| Check terminal after keypress | CROMINST prompt/menu appears |

### Fail classification

| Symptom | Likely issue |
|---|---|
| No `U` stream | SIC TX path to host failed |
| `U` stream present, keypress no effect | Host TX path to SIC failed |
| Prompt garbled/unstable | Level/timing/integrity issue |

## 4) Stability + Behavior Pass

| Item | Requirement |
|---|---|
| Duration | 30-minute active session |
| Interaction cadence | Harmless prompt interaction every 5 minutes |
| Log events | dropped/garbled chars, prompt stall/lockup, SIC reset, fan anomaly onset minute |

## 5) Repeatability Pass

| Step | Pass condition |
|---|---|
| Controlled disconnect/reconnect | Reconnect successful |
| Re-run handshake | Same behavior as initial run |

## 6) Run Validity Rules

| Rule | Requirement |
|---|---|
| Safety gate | Passed |
| Handshake | Full pass (`U` -> keypress -> prompt) |
| Stability | 30-minute session completed |
| Procedure integrity | No mid-run rewiring |

Runs failing any rule are logged as invalid and excluded from winner scoring.
