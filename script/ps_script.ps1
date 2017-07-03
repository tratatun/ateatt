#E:\_w\atea\_git\script\ps_script.ps1 'ttn' 'Da1mpDa2mp' 'ttn' 

$timeout = new-timespan -Days 30
$sw = [diagnostics.stopwatch]::StartNew()
while ($sw.elapsed -lt $timeout){
    $username = $args[0]
    $password = $args[1]
    $compName = $args[2]
    $APIURL = 'http://atea-api.azurewebsites.net/api/appsinfo' 
    $cred = New-Object System.Management.Automation.PSCredential -ArgumentList @($username,(ConvertTo-SecureString -String $password -AsPlainText -Force))
    #$user = Get-Credential
    $jsonData = Invoke-Command $compName -Credential $cred -ScriptBlock {Get-ItemProperty HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\*, HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\* | select DisplayName, Publisher, DisplayVersion, InstallDate, PSComputerName | ?{$_.DisplayName -notlike "" } }| ConvertTo-Json

    Invoke-RestMethod -Uri $APIURL -Body $jsonData -Method Post -ContentType 'application/json; charset=utf-8'

    # wait 30 minutes
    start-sleep -seconds $(30*60)
}
 
write-host "Timed out"