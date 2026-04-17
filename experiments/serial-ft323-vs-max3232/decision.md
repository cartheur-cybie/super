# Decision Sheet (FT323 vs MAX3232)

Complete this after at least 5 valid runs per path on the same SIC unit set.

## Hard Gates (non-negotiable)

* Any confirmed unsafe SIC-facing logic level disqualifies that path.
* Any repeatable SIC instability (reset/lockup/fan anomaly) disqualifies that path until root cause is proven external.

## Evidence Summary

| Metric | FT323 | MAX3232 | Preferred |
|---|---:|---:|---|
| Valid runs completed | TBD | TBD | TBD |
| Full handshake pass (`U` -> keypress -> prompt) | TBD | TBD | TBD |
| 30-minute stability pass count | TBD | TBD | TBD |
| Reconnect pass count | TBD | TBD | TBD |
| Fan anomaly count | TBD | TBD | TBD |
| Garble/timeout incidents | TBD | TBD | TBD |
| Safety gate failures | TBD | TBD | TBD |

## Notes Linked to Prior Repo Findings

* MAX233A 5V-risk finding from `ProjectNotes.txt` must remain closed (no regressions).
* FTDI fan-behavior issue from `ProjectNotes.txt` must be explicitly confirmed resolved before selecting FT323 as default.

## Final Selection

* Chosen default path:
* Why this wins on SIC behavior and bench reliability:
* Exact parts/cable mapping to standardize:
* Conditions where the alternate path is still acceptable:
