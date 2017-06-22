$username = 'TTN\ttn'
$password = 'Da1mpDa2mp'
$compName = 'ttn'
$hostAPI = 'http://localhost:5000'
$APIURI = '/api/values'
$cred = New-Object System.Management.Automation.PSCredential -ArgumentList @($username,(ConvertTo-SecureString -String $password -AsPlainText -Force))
#$user = Get-Credential
$jsonData = Invoke-Command $compName -Credential $cred -ScriptBlock {Get-ItemProperty HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\* | select DisplayName, Publisher, DisplayVersion, InstallDate | ?{$_.DisplayName -notlike "" } | ft}| ConvertTo-Json

Invoke-RestMethod -Uri ($hostAPI + $APIURI) -Body '45' -Method Post -ContentType 'application/json'

#$tr = {value = $outVar| ConvertTo-Json}