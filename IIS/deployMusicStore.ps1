$siteDeployPackagePath = (Get-Item '.\MvcMusicStore.zip').FullName
$configSectionsToEncrypt = @()

$siteName = 'MvcMusicStore'
$physicalSitePath = 'c:\test\musicStore'
$ipAddress = '*'
$port = 81
$hostName = 'web.bekk.Fagdag2011.no'
$appPoolName = 'MusicStoreAppPool'
$ravenSiteName = 'MusicStoreRavenDb'
$ravenPhysicalSitePath = 'c:\test\ravenDb'
$ravenIpAddress = '*'
$ravenHostName = 'web.bekk.Fagdag2011.no'
$ravenPort = 82
$ravenAppPool = 'RavenDbAppPool'
$backupPath = 'c:\test\backup'


$command = '.\runDeploy.ps1 -webApplicationSiteName $siteName -physicalSitePath $physicalSitePath -ipAddress $ipAddress -port $port -hostName $hostName -appPoolName $appPoolName  -siteDeployPackagePath $siteDeployPackagePath -configSectionsToEncrypt $configSectionsToEncrypt -ravenSiteName $ravenSiteName -ravenSitePath $ravenPhysicalSitePath -ravenSiteIpAddress $ravenIpAddress -ravenSiteHostName $ravenHostName -ravenSitePort $ravenPort -ravenAppPoolName $ravenAppPool -siteBackupLocation $backupPath'
Invoke-Expression $command