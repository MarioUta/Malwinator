### [C2 Server](https://github.com/MarioUta/Malwerinator-educational-/tree/main/C2_Server "Check the server code")
The C2 server provides a web interfece to enable the threat actor to perform various actions: execute shell commands on the host, open the host's webcam, record what the host is typing and exchange files between the server and the host. In order to access the web interface, the browser must have a cookie named ```superSecretKey``` with the value set to ```c457r4v371```.

### [Testing](https://github.com/MarioUta/Malwerinator-educational-/tree/main/Testing "Check the scripts")

The testing process focused on the upload feature of the malware.  
      
By using the unittest and Selenium libraries, the tesing scripts check
whether a file is sent if given a valid Path and whether all the sent 
files arrive at the given destination.


   
      
### [Camera](https://github.com/MarioUta/Malwerinator-educational-/tree/main/Camera "Check the camera code")

  
The camera sender module sends camera frames in rapid succession as byte streams, which are then rendered in browser as images, changing source with each new frame sent.

