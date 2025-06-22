# 🧱Architecture of the project
![image](https://github.com/user-attachments/assets/4a18132c-e328-4e19-8c4c-8e0ca543fa62)
this project is split into 2 parts. I've done this because [btleplug](https://github.com/deviceplug/btleplug) is the only bluetooth library that worked for me and is multiplatform. it also splits project into 2 parts nicely.  
### [Rust](https://github.com/FilipRuman/4Form-BluetoothHandler) 
 it's responsible for interacting with devices thru Bluetooth low energy with [btleplug](https://github.com/deviceplug/btleplug). sends data and collects inputs thru TCP.
### Godot \ C#
 collects data from devices and sends inputs thru tcp to rust. all game tools are written in C#. 

# 🗺️Map exporting:
![image](https://github.com/user-attachments/assets/ef2fc1b6-7703-4d45-818b-963d27532721)

The main goal is SAFTEY so i don't use any tres files.
Also this could be expanded further in the future to allow custom materials and other user content.
