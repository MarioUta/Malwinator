# Implant

## Description

The Implant is designed to look as an antivirus solution that must be run as Administrator to work. On the surface, it will show on the user's screen a popup that tells them that one application is installing and another one that the PC will restart in order to complete some updates after cleanup, then restarting the PC. However, unbeknownst to them, after the PC reboots, the virus is now up and running on their PC, because the Implant adds the program to Startup.
It will not be captured because the implant adds the entire C: drive, alongside "Desktop" and "Downloads" to the Windows Defender exclusion path, thus not scanning anything at all. The Virus app will also be converted into a hidden item, so it will not be shown on the Desktop(or anywhere, really) unless the user enables hidden items to be shown. 
The aim of the Implant is to hide the virus in plain sight so that, even if the user deletes the Implant, they will not feel the effects of the virus until it's too late. 


## Usage: 

#### Requirements

The executable from C2_Client has to be compiled and present on the /resources folder on the C2_Sever for the script to work.

### 1)Install module ps2exe
Warning: Start PowerShell as administrator

```PowerShell
Install-Module -Name ps2exe -Scope CurrentUser
```

### 2)Executable creation
Warning: Start PowerShell as administrator

Create the PowerShell script
```PowerShell
win-ps2exe
```

Then, a graphical interface pops up:
Source file: Path\To\Executable.ps1 (if you install the executable only)
Destination file: Desktop (automatically makes executable with the same name)
Icon file: Path\To\Icon.ico(an .ico file with your choosing)
Version and Product name: what you want in order to make it look legit

DO NOT ADD ANY COPYRIGHT, IT WILL NO LONGER WORK

Then press Compile. the executable is now ready.
### 3) RUN IT AS ADMINISTRATOR
And that's it. the Implant is now functional


