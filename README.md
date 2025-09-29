## Super-iCybie

An upgrade path to of increased utility via connectivity and development.

![lab](/images/evening-lab.png)

### Getting Started

There are several methods to get started, but the most elightening is the removal of the outer shells to get a look how the robot is constructed.

![internals](/images/no-covers.jpg)

### Working

Originally followed [this](https://aibohack.com/icybie/sic_rs232.htm) source. The layout is demonstrated as:

![layout](/images/connections.jpg)

### Communication

_RS-232_

Using the MAX233 and the USB-Serial [convertor](https://support.eminent-online.com/hc/en-us/articles/360009538439-EM1016-Download-Drivers). Details regarding its use will be best-utilized on a Windows machine.

![connection](/images/rs232.jpg)

Where the power connection is only needed if using the MAX233.

![power](/images/powerconnect.jpg)

_FTDI_

Using the FT232RL and direct connectivity to the installed serial port these [drivers](https://ftdichip.com/drivers/d2xx-drivers/) are needed. Details regarding its use on RaspberryPi will be discussed.

![ft232rl](/images/FT232RL.png)

Where the connection between `Tx` and `Rx` on the CMOS-level system and the iCybie is:

![cable](/images/cablecyble.jpg)

### Motherboard

The iCybie motherboard is exhibited as a property of location of element on each side:

![top](/images/boardtop.jpg)

![bottom](/images/boardbottom.jpg)

Where the connection between a serial port terminal and the iCybie is through a serial port on-board:

![serial](/images/serialconnect.jpg)

### Specifications

These are listed [here](/spec/README.md).