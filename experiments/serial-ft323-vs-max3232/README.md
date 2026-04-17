# FT323 vs MAX3232 Comparative (SIC-Specific)

This experiment set compares two connector paths for Super i-Cybie serial access:

* `FT323/FT232R` direct TTL path (external USB serial adapter)
* `MAX3232` level-shifted RS-232 path (AiboHack-style serial chain)

This is not a generic test track. It is grounded in project history:

* `ProjectNotes.txt` (2025-10-05): MAX233A path observed as unsafe due to 5V TTL risk.
* `ProjectNotes.txt` (2025-10-06): confirmed cable mapping workflow (`Tx`, `Rx`, `GND`) with continuity and voltmeter.
* `ProjectNotes.txt` (2026-04-17): FTDI path observed with abnormal fan behavior.
* `docs/RS-232 Installation and Test.pdf`: reference serial validation sequence (`9600 8N1`, repeating `U` stream, keypress to CROMINST prompt).

## Goal

Select the default SIC connector technology for day-to-day programming and diagnostics based on:

* Electrical safety at SIC pins.
* Stable bidirectional serial behavior.
* Absence of robot side effects (especially fan anomalies).
* Repeatable setup with low bench friction.

## Comparator Definition

* Path A: `FT323/FT232R` direct-to-SIC TTL wiring (3.3V logic only; no 5V logic exposure).
* Path B: `MAX3232` path via stereo jack/DB9 mapping and RS-232 translation.

## Canonical Validation Signal

Both paths must pass the same SIC handshake test from CROMINST context:

1. SIC sends repeating `U` bytes at `9600 8N1`.
2. Host keypress stops `U` stream.
3. CROMINST command prompt appears.

Only runs that pass all 3 are considered valid for comparison.

## Files

* `test-protocol.md` - exact per-run steps and fail gates.
* `results-log.md` - structured run capture with SIC-specific observations.
* `decision.md` - evidence summary and path recommendation.

## Photo References

Use these during bench setup to reduce wiring mistakes:

* SIC internal connection layout:
  ![SIC serial layout](../../images/connections.jpg)
* On-board serial connection area:
  ![SIC serial connector area](../../images/serialconnect.jpg)
* FT232RL reference board:
  ![FT232RL board](../../images/FT232RL.png)
* TTL cable orientation reference:
  ![TTL cable reference](../../images/cablecyble.jpg)
* RS-232 DB9 pinout:
  ![DB9 pinout](../../rs232-ftdi/DB9-Pinout.jpg)
* Prior ops bench reference:
  ![Ops test reference](../../rs232-ftdi/ops-test.jpg)
