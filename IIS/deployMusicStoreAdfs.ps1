$siteDeployPackagePath = (Get-Item '.\MvcMusicStoreAdfs.zip').FullName
$configSectionsToEncrypt = @()

$siteName = 'MvcMusicStoreAdfs'
#$physicalSitePath = Read-Host 'Physical path of the application web site'
$physicalSitePath = 'c:\test\musicStoreAdfs'
$ipAddress = '*'
$port = 83
$hostName = 'WIN-IA6KB8UL431.da.domain.test'
$appPoolName = 'MusicStoreAdfsAppPool'
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
$adfsCertificateFilePath = 'c:\adfsCertificate.cer'
$adfsHostName = 'win-ia6kb8ul431.da.domain.test'


$command = '.\runDeploy.ps1 -webApplicationSiteName $siteName -physicalSitePath $physicalSitePath -ipAddress $ipAddress -port $port -hostName $hostName -appPoolName $appPoolName -certificateSubject $certificateSubject  -siteDeployPackagePath $siteDeployPackagePath -configSectionsToEncrypt $configSectionsToEncrypt -ravenSiteName $ravenSiteName -ravenSitePath $ravenPhysicalSitePath -ravenSiteIpAddress $ravenIpAddress -ravenSiteHostName $ravenHostName -ravenSitePort $ravenPort -ravenAppPoolName $ravenAppPool -siteBackupLocation $backupPath -adfsCertificateFilePath $adfsCertificateFilePath -adfsHostName $adfsHostName'
Invoke-Expression $command