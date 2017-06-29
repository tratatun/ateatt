$username = $args[0]
$password = $args[1]
$compName = $args[2]
$APIURL = 'http://atea-api.azurewebsites.net/api/values'
$cred = New-Object System.Management.Automation.PSCredential -ArgumentList @($username,(ConvertTo-SecureString -String $password -AsPlainText -Force))
#$user = Get-Credential
$jsonData = Invoke-Command $compName -Credential $cred -ScriptBlock {Get-ItemProperty HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\*, HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\* | select DisplayName, Publisher, DisplayVersion, InstallDate, PSComputerName | ?{$_.DisplayName -notlike "" } }| ConvertTo-Json

Invoke-RestMethod -Uri $APIURL -Body $jsonData -Method Post -ContentType 'application/json; charset=utf-8'
