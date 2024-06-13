$Handshake = 'http://localhost:5000/handshake'
$Log = 'http://localhost:5000/log'

$Form = @{
  name  = $env:COMPUTERNAME
}

$Result = Invoke-WebRequest -Uri $Handshake -Method Post -Body $Form

while ($true) {
  for ($char = [int][char]'a'; $char -le [int][char]'z'; $char++) {
    $currentChar = [char]$char
    $Form['key'] = $currentChar
    # Write-Host $currentChar
    $Result = Invoke-WebRequest -Uri $Log -Method Post -Body $Form
    Start-Sleep -Seconds 1
  }
}