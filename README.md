### [C2 Server](https://github.com/MarioUta/Malwerinator-educational-/tree/main/C2_Server "Check the server code")
The C2 server provides a web interfece to enable the threat actor to perform various actions: execute shell commands on the host, open the host's webcam, record what the host is typing and exchange files between the server and the host. In order to access the web interface, the browser must have a cookie named ```superSecretKey``` with the value set to ```c457r4v371```.

### [Testing](https://github.com/MarioUta/Malwerinator-educational-/tree/main/Testing "Check the scripts")

The testing process focused on the upload feature of the malware.  
      
By using the unittest and Selenium libraries, the tesing scripts check
whether a file is sent if given a valid Path and whether all the sent 
files arrive at the given destination.


   
      
### [Camera](https://github.com/MarioUta/Malwerinator-educational-/tree/main/Camera "Check the camera code")

  
The camera sender module sends camera frames in rapid succession as byte streams, which are then rendered in browser as images, changing source with each new frame sent.

### [Implant](https://github.com/MarioUta/Malwerinator-educational-/tree/main/EAE)

The Implant is designed to look as an antivirus solution that must be run as Administrator to work. On the surface, it will show on the user's screen a popup that tells them that the PC will restart in order to complete some updates after cleanup(or a video if you download the implant with video and place the video on desktop), then restarting the PC. However, unbeknownst to them, after the PC reboots, the virus is now up and running on their PC, because the Implant adds the program to Startup.
It will not be captured because the implant adds the entire C: drive, alongside "Desktop" and "Downloads" to the Windows Defender exclusion path, thus not scanning anything at all. The Virus app will also be converted into a hidden item, so it will not be shown on the Desktop(or anywhere, really) unless the user enables hidden items to be shown. 
The aim of the Implant is to hide the virus in plain sight so that, even if the user deletes the Implant, they will not feel the effects of the virus until it's too late. 
