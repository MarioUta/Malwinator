The Implant is designed to look as an antivirus solution that must be run as Administrator to work. On the surface, it will show on the user's screen a popup that tells them that the PC will restart in order to complete some updates after cleanup(or a video if you download the implant with video and place the video on desktop), then restarting the PC. However, unbeknownst to them, after the PC reboots, the virus is now up and running on their PC, because the Implant adds the program to Startup.
It will not be captured because the implant adds the entire C: drive, alongside "Desktop" and "Downloads" to the Windows Defender exclusion path, thus not scanning anything at all. The Virus app will also be converted into a hidden item, so it will not be shown on the Desktop(or anywhere, really) unless the user enables hidden items to be shown. 
The aim of the Implant is to hide the virus in plain sight so that, even if the user deletes the Implant, they will not feel the effects of the virus until it's too late. 

# Testing:

After testing, I decided to try and craft a trojan horse that must be run as administrator to work, disguised as an Antivirus app. The Icon should appear legit and when run it should not just open the virus itself, rather create an environment for the virus to wreak havoc

# Plans:
 a) Add specific paths, such as Desktop/Download in Windows Defender exclusion paths(success)
 b) Open up a process(a video for fun :D ), or show a popup, then restart the PC(success)
 c) Search for the virus path in order to manipulate it(success)
 d) Add virus to Startup and make executable hidden(success)
 e) Looks like an App (success)


