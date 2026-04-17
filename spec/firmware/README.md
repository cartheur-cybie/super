# Impact of the i-Cybie 88011 Motherboard on Firmware and Cartridges

The i-Cybie model 88011, manufactured by Silverlit Toys Manufactory Ltd. and distributed by Tiger Electronics (owned by Hasbro), uses a specific motherboard that influences its firmware and cartridge compatibility, particularly with its support for dual-chip cartridges. This section details the implications for firmware functionality and cartridge programming, focusing on the 88011 model’s capabilities and limitations.

## Firmware

- **Firmware Ownership and Design**: The i-Cybie firmware was developed by Micom Tech HK for Tiger Electronics, with Hasbro owning the rights after acquiring Tiger. The 88011 model, an early version of the i-Cybie, likely uses an earlier firmware iteration compared to later models (88012 and 88013). While minor differences in component arrangement or circuit board versions may exist, these are not known to significantly affect firmware functionality.
- **88011 Motherboard Impact**: The 88011 motherboard supports the i-Cybie’s core functionality, including:
  - **Toshiba TMP91C815F**: Handles motion control and mood calculation, powered by a regulated voltage derived from VBATX (6V, yellow wire).
  - **SunPlus Technology CPU**: Manages audio playback, also powered via regulated VBATX.
  - **RSC 364**: Dedicated to voice recognition and recording, enabling user interaction through voice commands.
  
  The firmware manages the robot’s 16 motor drives for movement (legs, head, tail), sensor interactions (sound, touch, infrared, light, orientation), and autonomous behaviors like charging with the appropriate cartridge. The 88011’s firmware is compatible with dual-chip cartridges, facilitating programming and customization.

- **Hacking Potential**:
  - The 88011 model is hackable to create a **Super i-Cybie (SIC)**, allowing direct cartridge programming without a Silverlit Downloader. This involves soldering a communications port near the main CPU (Toshiba TMP91C815F) and installing a bootloader (**CROMINST**) on a programmable cartridge.
  - The Super i-Cybie setup enables the robot to program other i-Cybies or be reprogrammed with new personalities. Unlike the Outrageous International model (88013), the 88011 is confirmed compatible with this hack.
  - The firmware can be fully reprogrammed in C using the user-developed **i-Cybie Software Development Kit (ICSDK)**, allowing custom behaviors or enhanced functionality.

## Cartridges

- **Dual-Chip Cartridges**:
  - The 88011 motherboard is designed to work with programmable cartridges using two 1-Mbit **SST39vf010** chips, providing a total of 256KB flash ROM. These cartridges store additional movement data and audio files, enabling custom behaviors or personalities (e.g., the Z-Cybie personality included in later production runs).
  - The 88011 is fully compatible with dual-chip cartridges, making it ideal for programming via a **Silverlit Downloader** or a Super i-Cybie.

- **Single-Chip Cartridge Compatibility**:
  - Later i-Cybie models or cartridges (post-2004) used a single 2-Mbit **SST39vf200** chip, which cannot be programmed using a standard Super i-Cybie based on the 88011 model without modifications.
  - An updated Super i-Cybie configuration, with the latest bootloader or ICSKD tools, can program single-chip cartridges, allowing the 88011 to support newer cartridges with appropriate upgrades.

- **YICT Programming**:
  - The **YICT (Yanni Idolizes iCybie Tweeking)** program, developed by “Aibopet,” is a key tool for programming i-Cybie cartridges, particularly for the 88011 model.
  - YICT version 2.02 supports multiple personalities (e.g., Z/2 personality) and allows users to add actions to each mood (happy, hyper, sad, sleepy, sick) or modify overall behavior.
  - The 88011’s compatibility with dual-chip cartridges makes it well-suited for YICT programming, enabling features like autonomous charging or new movement sequences via a Silverlit Downloader or Super i-Cybie.

## Practical Implications

- **Firmware Stability**:
  - The 88011’s firmware supports all standard i-Cybie features, including voice recognition, sensor interactions, and autonomous charging (with the appropriate cartridge).
  - To verify firmware version, observe the robot’s behavior or test cartridge compatibility. Updating firmware requires a Super i-Cybie setup and ICSKD, which is feasible with the 88011 model.

- **Cartridge Compatibility**:
  - Ensure cartridges are dual-chip (SST39vf010) for guaranteed compatibility with the 88011 motherboard. Single-chip cartridges (SST39vf200) may require an updated Super i-Cybie or additional hacking.
  - Check cartridge labels or test with YICT to confirm chip type before programming.

- **Hacking for Enhanced Functionality**:
  - To maximize the 88011’s potential, hack it into a Super i-Cybie by soldering a communications port near the main CPU and installing CROMINST. This enables direct cartridge programming and firmware customization without a Silverlit Downloader.
  - Detailed instructions are available through **AIBOhack** (search for “Aibopet i-Cybie”) or **YICT communities**.

## Recommendations

- **Verify Cartridge Type**: Inspect cartridges to confirm whether they are dual-chip (SST39vf010) or single-chip (SST39vf200). Source dual-chip cartridges from vintage marketplaces (e.g., eBay, Etsy) or Silverlit’s 3D Eshop (if available).
- **Join Enthusiast Communities**: Engage with **Synthiam** (synthiam.com) or **AIBOhack** forums to access hacking guides for the 88011 model, including Super i-Cybie modifications and YICT programming tutorials.
- **Test Voltages**: Confirm the power system voltages (VBAT: 12V, red wire; VBATX: 6V, yellow wire; ground: 0V, black wire) with a multimeter to ensure the 88011 motherboard and battery are functioning correctly.
- **Source Documentation**: If official schematics are unavailable from **Silverlit** (www.silverlit.com) or **Hasbro** (www.hasbro.com), explore **Synthiam**, **AIBOhack**, or **Elektrotanya** (elektrotanya.com) for reverse-engineered schematics. Consider tracing the 88011 motherboard’s VBAT and VBATX lines to create a partial schematic if needed.

---
