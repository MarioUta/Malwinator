$Handshake = 'http://localhost:5000/handshake'
$Ready = 'http://localhost:5000/ready'

$Form = @{
  name  = $env:COMPUTERNAME
}

$Result = Invoke-WebRequest -Uri $Handshake -Method Post -Body $Form

while ($true) {
  $Result = Invoke-WebRequest -Uri $Ready -Method Post -Body $Form
  Write-Output $Result.Content
}