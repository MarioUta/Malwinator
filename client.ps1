$Handshake = 'http://localhost:5000/handshake'
$Ready = 'http://localhost:5000/ready'
$Pong = 'http://localhost:5000/pong'

$Form = @{
  name  = $env:COMPUTERNAME
}

$Result = Invoke-WebRequest -Uri $Handshake -Method Post -Body $Form

while ($true) {
  try {
    $Result = Invoke-WebRequest -Uri $Ready -Method Post -Body $Form
  }
  catch [System.SystemException] {
    Exit
  }
  
  Write-Output $Result.Content

  if ($Result.Content -eq 'ping'){
    $Result = Invoke-WebRequest -Uri $Pong -Method Post -Body $Form
  }

  Write-Output $Result.Content
}