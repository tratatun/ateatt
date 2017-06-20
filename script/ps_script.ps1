$username = 'user1'
$password = 'pAssw0rd'
$cred = New-Object System.Management.Automation.PSCredential -ArgumentList @($username,(ConvertTo-SecureString -String $password -AsPlainText -Force))
#$user = Get-Credential
Invoke-Command -uri HomePC -Credential $cred -ScriptBlock {Get-ItemProperty HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\* | select DisplayName, Publisher, DisplayVersion, InstallDate | ?{$_.DisplayName -notlike "" } | ft} 
#Invoke-RestMethod
