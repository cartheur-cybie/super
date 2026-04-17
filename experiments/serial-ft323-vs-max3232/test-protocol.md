# Test Protocol (FT323 vs MAX3232)

## 0) Controls (constant across both paths)

* Same SIC unit for A/B pair (start with Betix if CROM-confirmed).
* Same host machine, terminal app, and USB port where possible.
* Terminal settings fixed: `9600 baud`, `8 data bits`, `no parity`, `no handshake`.
* Same power arrangement per pair (do not change bench power mid-comparison).

## 1) Preflight Safety Gate (mandatory)

Photo checkpoints:

* Internal layout reference: `../../images/connections.jpg`
* Serial area close-up: `../../images/serialconnect.jpg`
* RS-232 pinout reference: `../../rs232-ftdi/DB9-Pinout.jpg`

### 1.1 Wiring identity

* Verify continuity for each conductor end-to-end before connecting SIC.
* Confirm no shorts between signal and ground.

### 1.2 SIC-side signal mapping

Use your known cable assignment from `ProjectNotes.txt` (2025-10-06 12:49):

* `Tx -> tip`
* `Rx -> center`
* `GND -> base`

If a cable or adapter differs, document the exact mapping in the run notes.

### 1.3 Voltage gate

* Measure idle TX level at adapter output.
* Stop run if SIC-facing logic level is not in safe 3.3V domain.
* For RS-232 side checks, use AiboHack-style expectation that PC TX/RX on DB9 read negative idle voltage.

## 2) Path Setup

### 2.1 Path A - FT323/FT232R direct TTL

* Connect only `TX`, `RX`, `GND` (no VCC feed to SIC serial header).
* Confirm `adapter TX -> SIC RX` and `adapter RX -> SIC TX`.
* If using FTDI EEPROM profile, record whether `drivers/iCybie-Serial.xml` config is applied.
* Photo references:
  * FT232RL board: `../../images/FT232RL.png`
  * TTL lead orientation: `../../images/cablecyble.jpg`

### 2.2 Path B - MAX3232 RS-232 chain

* Build/use MAX3232 translation path in place of prior MAX233A path.
* Validate stereo jack to DB9 mapping before SIC connection:
  * `ground -> DB9 pin 5`
  * `tip -> DB9 pin 2` (iCybie -> PC direction)
  * `ring/center conductor -> DB9 pin 3` (PC -> iCybie direction)
* Confirm no accidental MAX233A insertion in this path.
* Photo references:
  * RS-232 connection concept: `../../images/rs232.jpg`
  * DB9 pinout: `../../rs232-ftdi/DB9-Pinout.jpg`

## 3) SIC Handshake Test (primary comparator)

1. Insert CROMINST-capable cartridge/setup.
2. Connect serial path under test.
3. Power SIC.
4. Observe repeating `U` stream.
5. Press any host key.
6. Confirm `U` stream stops and CROMINST menu/prompt appears.

Fail classification:

* No `U` stream: TX path from SIC to host failed.
* `U` stream present but keypress no effect: RX path from host to SIC failed.
* Prompt unstable/garbled: timing/level integrity issue.

## 4) Stability + Behavior Pass

* Keep session active for 30 minutes.
* Every 5 minutes send a harmless keypress/command prompt interaction.
* Track:
  * dropped/garbled characters
  * stalled prompt or lockup
  * SIC resets
  * abnormal fan behavior (explicitly note onset minute)

## 5) Repeatability Pass

* Disconnect/reconnect once.
* Re-run handshake test.
* Pass only if post-reconnect behavior matches initial run.

## 6) Run Validity Rules

A run is valid only if:

* preflight safety gate passed,
* handshake test fully passed,
* 30-minute session completed,
* no mid-run rewiring occurred.

Invalid runs are logged but excluded from winner scoring.
