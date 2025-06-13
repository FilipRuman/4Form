<p align="center">  
  <img src="https://github.com/user-attachments/assets/b077f39c-11fc-4e06-9594-d069614ca01b" alt="logo" width="500">  
</p>

# 4Form
## Community driven, [open source](https://github.com/FilipRuman/4Form/blob/main/LICENSE) bike trainer app.
4Form allows users to create interesting training in the exact way they like by allowing community to create:
 * maps
 * routes
 * equipment
 * game modes
 * competitions
**Base** Of this project will be free and open source **forever**.
## If you think this project sounds interesting, you might use it in the future when it's production ready, Please give this repo a star, it helps a lot.
## This project is in really early stage of development 
It will change a lot in the future and **MOST** of the intended features are not implemented *yet*.  
## Example Vid:
## Implemented features
 * **easy to use tools for routes creation** - create routes using path3D and custom tool makes it smoothly follow terrain.
 * **integration with [terrain3D](https://github.com/TokisanGames/Terrain3D)** - allows easy creation of great looking maps. 
 * **multi-platform** - windows 11 && 10??, mac, and *****linux***(most distros if you do some tinkering)
 
## Future plans
 * steam release
 * steam workshop and easy maps downloading
 * multiplayer with steam and steam workshop to download maps
 * custom scripts in users content

### [For more accurate progress tracking use development project](https://github.com/users/FilipRuman/projects/6/views/3) 

## If you want to test it out just download [the latest release](https://github.com/FilipRuman/4Form/releases) for your platform
## Setup for development:
### 0.a [Install rust](https://www.rust-lang.org/learn/get-started) 
### 0.b [Install latest Godot with .Net support](https://godotengine.org/download/)
### 1. clone this repo with sub-modules:
``git clone --recurse-submodules https://github.com/FilipRuman/4Form``
``git switch dev``
### 2. make sure that you have latest build of ble-handler
``cd 4Form/subomdules/4Form-BluetoothHandler``
``cargo build``
### 3. open project in Godot
### You are ready to run it!

## Architecture of project
![image](https://github.com/user-attachments/assets/4a18132c-e328-4e19-8c4c-8e0ca543fa62)
this project is split into 2 parts:
### [Rust](https://github.com/FilipRuman/4Form-BluetoothHandler) 
 it's responsible for interacting with devices thru Bluetooth low energy with [btleplug](https://github.com/deviceplug/btleplug). sends data and collects inputs thru TCP.
### Godot \ C#
 collects data from devices and sends inputs thru tcp to rust. all game tools are written in C#. 

# [Contribution guide](./CONTRIBUTING.md) 
