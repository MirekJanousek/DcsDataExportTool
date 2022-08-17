# DCS Data Export Tool

## Disclaimer
This software is under development and not yet completed. Any DCS update can break it's functionality and render it not-working. This tool doesn't manipulate with the data in any way, doesn't change game files and should not harm your DCS installation. Code is publicly available and not working with any sensitive data.

## What is it good for?
Export tool to get DCS modules clickable data from installed script files (installed modules)

Command Identifiers and values can than be used for creating the buttons on your Stream Deck via **[DCS Interface for Stream Deck](https://github.com/enertial/streamdeck-dcs-interface)**

## What can I use it with?
Exported data are suitable for use with:

- **[DCS Interface for Stream Deck](https://github.com/enertial/streamdeck-dcs-interface)** by **[Charles Tytler](https://github.com/charlestytler)** 

## Installation
- Unzip the content of ZIP archive to any folder
- Change the path to you DCS folder in the **appsettings.json** (*root folder of DCS*)
```
  "ExportSettings": {
    "DcsFolderPath": "y:/Games/DCS World OpenBeta"
  }
```
- Change the target folder for exported CSV files in the **appsettings.json** (*default is C:/DcsExports*)
```
  "AppSettings": {
    "ExportDirectoryPath":  "c:/DcsExports"
  }
```

## How to use the application
Make sure you have filled the right paths into **appsettings.json** and than run the application. Then just follow the instructions on screen

## Supported modules
Below is the list of modules that are tested and working. Other modules might work as well, but that needs to be confirmed.
- A-10C II Tank Killer
- A-10C Warthog
- AH-64D BLK.II
- AJS37
- AV-8B N/A
- F-14
- F-16C bl. 50
- F-5E Tiger II
- F/A18C
- Ka-50 Black Shark
- M-2000C
- Mi-24P
- Mi-8MTV2
- Mig-21bis
- Mirage F1
- NS430
- TF-51D Mustang
- UH-1H Huey

## Used libraries and ideas
- This software uses some basic ideas of **[DCS Interface for Stream Deck](https://github.com/enertial/streamdeck-dcs-interface)** by **[Charles Tytler](https://github.com/charlestytler)** 
-  **[NLUA Interface](https://github.com/NLua/NLua)** for work with LUA scripts is being used

## Future development
- Polishing of the code
- Export to HTML page (with basic search capabilities if possible)
- Making sure other modules (*not listed on the list of tested ones*) are working
- Possibility of providing the export tool as library if there is demand

## FAQ

### Why is the application as big? (*dozens of MBs*)
Application is written using Microsoft .NET technology, which requires the client's PC to have proper version of .NET installed. In order to not bother users with installation of the whole .NET package, this application contains all the necessary DLLs bundled into the EXE file. There are dozens of required libraries making the application bigger than one would expect for such a simple tool. 
