$username = 'TTN\ttn' #'alexander.nikiforov@softwarium.net'
$password = 'Da1mpDa2mp' #'Da1mpDa2mpDa1mp'
$compName = 'ttn' #'ttn.softwarium.net'
$APIURL = 'http://localhost:5000/api/values'
$cred = New-Object System.Management.Automation.PSCredential -ArgumentList @($username,(ConvertTo-SecureString -String $password -AsPlainText -Force))
#$user = Get-Credential
$jsonData = Invoke-Command $compName -Credential $cred -ScriptBlock {Get-ItemProperty HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\* | select DisplayName, Publisher, DisplayVersion, InstallDate, PSComputerName | ?{$_.DisplayName -notlike "" } }| ConvertTo-Json

Invoke-RestMethod -Uri $APIURL -Body $jsonData -Method Post -ContentType 'application/json; charset=utf-8'

#$tr = {value = $outVar| ConvertTo-Json}
#$jsonData| ConvertTo-Json = @{value = $jsonData}
