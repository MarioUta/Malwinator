In order to avoid windows defender detection while compiling it. make sure to disable the firewall and to exclude the folder from antivirus checking. Also it's recommended to add an icon to the executable in order for win defender to find it less suspicious. This is for compiling only. Windows 10 does not have a problem with running the already compiled executable, even with Windows defender on

Make sure to have opencv, pickle, struct and pyinstaller installed on pip.

To compile the script as an windows executable use the following command:

PyInstaller --onefile --noconsole --icon=<your_icon.ico> CameraClient.py