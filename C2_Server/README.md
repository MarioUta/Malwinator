# C2 Server
## Description
The server consists of a http server. It has endpoints for the thread actor (web interfaces) and for the C2 client. From the web interface the threat actor can perform various actions: execute shell commands on the host, open the host's webcam, record what the host is typing and exchange files between the server and the host. The server has 2 folders:
- downloads - where the C2 client send files from the host
- resources - here are the files the server can send to the client
Also the web interface allows the threat actor to upload a file to the resources folder from their own computer for ease of use.

## Usage
#### Requirements
```
pip install flask
pip install flask_socketio
```
#### Building and running
```
git clone -b c2 https://github.com/MarioUta/Malwerinator-educational-.git
cd Malwerinator-educational-/C2_Server
flask run
```
When uploading a file, the thread actor can choose a module. When uploading the ```keylogger.exe```(Keylogger) or ```sender.exe```(Webcam recorder) the threat actor must select the correct module in order for the C2 client to execute the correct commands when trying to open the camera or record the keys.
### Notes
All urls in the server source code point to http://malwinator.chickenkiller.com which is our own VPS we used during development, so if you want to run the server you will have to modify the srouce code for ```app.py``` and all ```.js``` files and for the C2 client.

In order to access the web interface, the browser must have a cookie named ```superSecretKey``` with the value set to ```c457r4v371```.
