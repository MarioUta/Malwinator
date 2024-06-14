# Camera

## Description 
This is a python application that opens the camera and starts sending the frame bytes over a websocket to an url specified as an argument.

## Usage

#### Requirements:

Pyinstaller
```
pip install pyinstaller
```
After installing make sure that pyinstaller is in your PATH environment variable or run the executable with the full path.

Folow the commands below:
```
git clone -b camera https://github.com/MarioUta/Malwinator.git
cd Malwerinator-educational-/Camera
pyinstaller --onefile .\sender.py
```
Now the executable should be created. The path to it is: ./dist/sender.exe
