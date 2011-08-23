$siteDeployPackagePath = (Get-Item '.\MvcMusicStoreAdfs.zip').FullName
$configSectionsToEncrypt = @()

$siteName = 'MvcMusicStoreAdfs'
$physicalSitePath = 'c:\test\musicStoreAdfs'
$ipAddress = '*'
$port = 83
$hostName = 'web.bekk.Fagdag2011.no'
$appPoolName = 'MusicStoreAdfsAppPool'
$certificateSubject = 'web-bekk-Fagdag2011-no'
$ravenSiteName = 'MusicStoreAdfsRavenDb'
$ravenPhysicalSitePath = 'c:\test\ravenDbAdfs'
$ravenIpAddress = '*'
$ravenHostName = 'web.bekk.Fagdag2011.no'
$ravenPort = 82
$ravenAppPool = 'RavenDbAdfsAppPool'
$backupPath = 'c:\test\backup'
$adfsCertificateFilePath = 'c:\adfsCertificate.cer'
#$adfsHostName = 'adfs.bekk.Fagdag2011.no'
$adfsHostName = Read-Host "Enter the URL of the ADFS-server"


$command = '.\runDeploy.ps1 -webApplicationSiteName $siteName -physicalSitePath $physicalSitePath -ipAddress $ipAddress -port $port -hostName $hostName -appPoolName $appPoolName -certificateSubject $certificateSubject  -siteDeployPackagePath $siteDeployPackagePath -configSectionsToEncrypt $configSectionsToEncrypt -ravenSiteName $ravenSiteName -ravenSitePath $ravenPhysicalSitePath -ravenSiteIpAddress $ravenIpAddress -ravenSiteHostName $ravenHostName -ravenSitePort $ravenPort -ravenAppPoolName $ravenAppPool -siteBackupLocation $backupPath -adfsCertificateFilePath $adfsCertificateFilePath -adfsHostName $adfsHostName'
Invoke-Expression $command