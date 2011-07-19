$siteDeployPackagePath = (Get-Item '.\MvcMusicStore.zip').FullName
$configSectionsToEncrypt = @()

$siteName = 'MvcMusicStore'
#$physicalSitePath = Read-Host 'Physical path of the application web site'
$physicalSitePath = 'c:\test\musicStore'
$ipAddress = '*'
$port = 81
$hostName = 'WIN-IA6KB8UL431.da.domain.test'
$appPoolName = 'MusicStoreAppPool'
$certificateSubject = 'WIN-IA6KB8UL431.da.domain.test'
$ravenSiteName = 'MusicStoreRavenDb'
#$ravenPhysicalSitePath = Read-Host 'Physical path of the raven database web site"
$ravenPhysicalSitePath = 'c:\test\ravenDb'
$ravenIpAddress = '*'
$ravenHostName = 'WIN-IA6KB8UL431.da.domain.test'
$ravenPort = 82
$ravenAppPool = 'RavenDbAppPool'
#$backupPath = Read-Host 'Path to the location for storing site bakups'
$backupPath = 'c:\test\backup'


$command = '.\runDeploy.ps1 -webApplicationSiteName $siteName -physicalSitePath $physicalSitePath -ipAddress $ipAddress -port $port -hostName $hostName -appPoolName $appPoolName -certificateSubject $certificateSubject  -siteDeployPackagePath $siteDeployPackagePath -configSectionsToEncrypt $configSectionsToEncrypt -ravenSiteName $ravenSiteName -ravenSitePath $ravenPhysicalSitePath -ravenSiteIpAddress $ravenIpAddress -ravenSiteHostName $ravenHostName -ravenSitePort $ravenPort -ravenAppPoolName $ravenAppPool -siteBackupLocation $backupPath'
Invoke-Expression $command