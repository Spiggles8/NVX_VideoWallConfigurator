# NVX Video Wall Configurator

## Overview

This repository contains the source code for the NVX Video Wall Configurator, a control system program for managing video wall panels and sources using Crestron 3-Series and above control processors and Crestron DM-NVX-360 Devices.

The program is divided into two main components:

- **Simpl# (Simpl Sharp):** The core logic of the program is implemented in Simpl#, a C#-based language supported by Crestron control systems. The Simpl# code provides the functionality for managing video wall panels and sources.

- **Simpl+ (Simpl Plus):** The Simpl+ code serves as the user interface and event handler for physical buttons and inputs. It interacts with the Simpl# code to control the video wall.

## Features

- Pressing panel buttons sets panels to "high" or "low" states.
- When a video input button is pressed, the program routes the video source and provides layout and position information to all high panels.
- Supports Crestron 3-Series and above control processors.
- Works with DM-NVX Receivers that support Video Wall capabilities.

### Prerequisites

To use this program, you'll need the following:

- A Crestron control system with a 3-Series or above control processor.
- DM-NVX Receivers with Video Wall capabilities.
- Crestron development tools, including Simpl Windows and VTPro.

### Installation

1. Clone this repository to your local machine.

2. Import the Simpl# and Simpl+ code into your Crestron development environment.

3. Compile and upload the program to your Crestron control processor.

4. Load the sample Simpl Windows program to simulate button presses and interactions.

5. Optionally, use the sample VTPro program to create a graphical user interface for the video wall control.

## Usage

1. Launch the Simpl Windows program to simulate button presses on the video wall panels and video inputs.

2. Observe the program's behavior as it sets panel states, routes video sources, and provides layout and position information to high panels.

3. Customize the program to suit your specific video wall configuration and control requirements.

## Contributing

Contributions to this project are welcome. Feel free to submit issues, bug reports, or pull requests if you have suggestions for improvements or fixes.

## Acknowledgments

- This project was developed by [Sean Spiggle].
- Special thanks to the Crestron community for their support and contributions.

## Sample Programs

Included in this repository are sample Simpl Windows and VTPro programs that demonstrate how to interact with the NVX Video Wall Configurator program. You can find these sample programs in the "SamplePrograms" directory.

- **NVX_VideoWallConfigurator_Sample.smw:** This Simpl Windows program provides a simulated interface for interacting with the video wall panels and video inputs.

- **NVX_VideoWallConfigurator_Sample_UI.vtp:** This VTPro project file includes sample touch panel designs for controlling the video wall. You can customize these designs to create a user-friendly interface for your control system.

## Future Updates
- I will continue modifying this to include functionality to more easily adjust the video wall size and dimensions
- Still working on testing with more systems and equipment to work out any bugs. 
