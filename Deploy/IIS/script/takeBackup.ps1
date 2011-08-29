Function BackupIisConfig($backupTimestamp){
	$iisBackupName = 'beforeDeploy_' + $backupTimestamp
	$windir = [System.Environment]::ExpandEnvironmentVariables("%WINDIR%")
	
	Write-Host
	Write-Host "Backup of IIS-configuration" -ForegroundColor Blue
	
	Push-Location
	Set-Location $windir'\system32\inetsrv\'

	Backup-WebConfiguration -Name $iisBackupName
	CheckForErrors

	Pop-Location
}

Function BackupSite ($siteName, $siteBackupLocation, $backupTimestamp, $MSDeploy){
	if(!$siteBackupLocation.EndsWith('\')){
		$siteBackupLocation += '\'
	}
	$backupFile = $siteBackupLocation + $siteName + '_' + $backupTimestamp +'.zip'
	
	if(DoesNotExistSite($siteName)){
		return
	}
	
	if(! (Test-Path $siteBackupLocation)){
		New-Item $siteBackupLocation -ItemType directory
		CheckForErrors
	}
	
	Write-Host
	Write-Host "Backup of Site " $siteName -ForegroundColor Blue
	
	.$MSDeploy -verb:sync -source:iisApp=$siteName -dest:package=$backupFile
	CheckForErrors
	
	Write-Host
	Write-Host "Backup was stored in file :" $backupFile
	
}