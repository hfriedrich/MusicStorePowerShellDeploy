﻿Function DeployWebApplicationSite($siteName, $physicalSitePath, $ipAddress, $port, $hostName, $appPoolName, $appPoolUserAccountName, $appPoolUserAccountPassword, $certificateSubject, $siteDeployPackagePath, $MSDeploy){
	$enabelSsl = ![string]::IsNullOrEmpty($certificateSubject)
	CreateAppPool $appPoolName
	SetAppPoolRuntimeVersion $appPoolName
	SetAppPoolIdentity $appPoolName $appPoolUserAccountName $appPoolUserAccountPassword $enabelSsl
	CreateSite $siteName $physicalSitePath $ipAddress $enabelSsl $port $appPoolName $certificateSubject
	
	.$MSDeploy -verb:sync -source:package=$siteDeployPackagePath -dest:auto -setParam:"kind='ProviderPath',scope='IisApp',value=$siteName"
	CheckForErrors
}

Function DeployRavenDbSite($siteName, $physicalSitePath, $ipAddress, $port, $hostName, $appPoolName, $appPoolUserAccountName, $appPoolUserAccountPassword, $siteContentSourcePath, $MSDeploy){
	CreateAppPool $appPoolName
	SetAppPoolRuntimeVersion $appPoolName
	SetAppPoolIdentity $appPoolName $appPoolUserAccountName $appPoolUserAccountPassword
	CreateSite $siteName $physicalSitePath $ipAddress $false $port $appPoolName $null

	.$MSDeploy -verb:sync -source:contentPath=$siteContentSourcePath -dest:contentPath=$physicalSitePath
	CheckForErrors
}

Function CreateAppPool($appPoolName){
	if(ExistsAppPool $appPoolName){
		return
	}
	
	Write-Host
	Write-Host "Creating new application pool: "$appPoolName -ForegroundColor Yellow
	
	New-WebAppPool -Name $appPoolName
	CheckForErrors
}

Function SetAppPoolRuntimeVersion ($appPoolName){
	Write-Host
	Write-Host "Setting runtime version of application pool"$appPoolName" to version 4.0" -ForegroundColor Blue
	
	Set-WebConfigurationProperty -Filter "/system.applicationHost/applicationPools/add[@name='$appPoolName']" -PSPath 'IIS:\' -Name 'managedRuntimeVersion' -Value 'v4.0'
	
	CheckForErrors
}

Function SetAppPoolIdentity ($appPoolName, $appPoolUserAccountName, $appPoolUserAccountPassword, $enabelSsl){
	Write-Host
	Write-Host "Setting identity of the application pool"$appPoolName" to "$appPoolUserAccountName -ForegroundColor Yellow
	
	Set-WebConfigurationProperty -Filter "/system.applicationHost/applicationPools/add[@name='$appPoolName']/processModel" -PSPath 'IIS:\' -name identitytype -value 3
	CheckForErrors
	Set-WebConfigurationProperty -Filter "/system.applicationHost/applicationPools/add[@name='$appPoolName']/processModel" -PSPath 'IIS:\' -name userName -value $appPoolUserAccountName
	CheckForErrors
	Set-WebConfigurationProperty -Filter "/system.applicationHost/applicationPools/add[@name='$appPoolName']/processModel" -PSPath 'IIS:\' -name password -value (ConvertToPlainText $appPoolUserAccountPassword)
	CheckForErrors	
	if($enabelSsl){
		Set-WebConfigurationProperty -Filter "/system.applicationHost/applicationPools/add[@name='$appPoolName']/processModel" -PSPath 'IIS:\' -name loadUserProfile -value $true
		CheckForErrors	
	}
}

Function CreateSite($siteName, $physicalSitePath, $ipAddress, $enabelSsl, $port, $appPoolName, $certificateSubject){
	if(ExistsSite($siteName)){
		Remove-Website -Name $siteName
		CheckForErrors
	}
	if(! (Test-Path $physicalSitePath)){
		New-Item $physicalSitePath -ItemType directory
		CheckForErrors
	}

	Write-Host
	Write-Host "Creating new site: "$siteName -ForegroundColor Yellow
	
	if($enabelSsl){
		New-Website -Name $siteName -PhysicalPath $physicalSitePath -IPAddress $ipAddress -Ssl -Port $port -HostHeader $hostName -ApplicationPool $appPoolName
		CheckForErrors
	}else{
		New-Website -Name $siteName -PhysicalPath $physicalSitePath -IPAddress $ipAddress -Port $port -HostHeader $hostName -ApplicationPool $appPoolName
		CheckForErrors
	}
	
	AssignCertificateToSiteBinding $certificateSubject $ipAddress $port
}

Function AssignCertificateToSiteBinding($certificateSubject, $ipAddress, $port){
	$certificate = GetCertificateFromCertificateProvider $certificateSubject
	if($certificate -eq $null){
		return
	}
	
	if($ipAddress -eq '*'){
		$ipAddress = '0.0.0.0'
	}
	$ipPortCombination = $ipAddress + '!' + $port	
	
	Push-Location
	Set-Location 'IIS:\SslBindings'
	
	if((Get-Item $ipPortCombination -ErrorAction SilentlyContinue) -ne $null){
		Remove-Item $ipPortCombination
		CheckForErrors
	}
	
	$certificate | new-item $ipPortCombination
	CheckForErrors
	Pop-Location
}

Function ExistsAppPool($appPoolName){
	$appPool = Get-Item "IIS:\AppPools\$appPoolName" -ErrorAction SilentlyContinue
	return ($appPool -ne $null)
}

Function GetCertificateFromCertificateProvider($certificateSubject){
	Push-Location
	cd cert:
	$allCertificates = ls .\LocalMachine\My
	$certificate = $allCertificates | where {$_.Subject -eq 'CN='+$certificateSubject}	
	Pop-Location
	return $certificate
}