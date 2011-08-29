Function ModifyRavenDbUrl($physicalSitePath, $ravenSiteUrl){
	$webConfig = $physicalSitePath + '\web.config'
   	$doc = new-object System.Xml.XmlDocument
   	$doc.Load($webConfig)
   	$addNodes = $doc.get_DocumentElement().connectionStrings.add
	foreach ($addNode in $addNodes){ 
		if($addNode.name -eq 'RavenDB'){ 
			$addNode.connectionString = $ravenSiteUrl.ToString()
		}
	}
   	$doc.Save($webConfig)
}

Function ModifyRavenAccessRights($physicalSitePath, $accessRights){
	$webConfig = $physicalSitePath + '\web.config'
   	$doc = new-object System.Xml.XmlDocument
   	$doc.Load($webConfig)
   	$addNodes = $doc.get_DocumentElement().appSettings.add
	foreach ($addNode in $addNodes){ 
		if($addNode.key -eq 'Raven/AnonymousAccess'){ 
			$addNode.value = $accessRights.ToString()
		}
	}
   	$doc.Save($webConfig)
}

Function SetIdentityModelUrlsAndCertificates($hostName, $certificateSubject, $requireSSL, $port, $adfsCertificateFilePath, $adfsHostName){
	$siteUrl = BuildWebSiteUrl $hostName $port $requireSSL
	$adfsCertificate = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2 (Get-Item -Path $adfsCertificateFilePath).FullName
	
	$webConfig = $physicalSitePath + '\web.config'
   	$doc = new-object System.Xml.XmlDocument
   	$doc.Load($webConfig)
   	$audienceUriNode = $doc.get_DocumentElement()."microsoft.identityModel".service.audienceUris.add
	if($audienceUriNode -ne $null){
		$audienceUriNode.value = $siteUrl.ToString()
	}
	
	$wsFederationNode = $doc.get_DocumentElement()."microsoft.identityModel".service.federatedAuthentication.wsFederation
	if($wsFederationNode -ne $null){
		$wsFederationNode.issuer = (BuildAdfsUrl $adfsHostName).ToString()
		$wsFederationNode.realm = $siteUrl.ToString()
		$wsFederationNode.requireHttps = $requireSSL.ToString()
	}
	
	$cookieHandlerNode = $doc.get_DocumentElement()."microsoft.identityModel".service.federatedAuthentication.cookieHandler
	if($cookieHandlerNode -ne $null){
		$cookieHandlerNode.requireSsl = $requireSSL.ToString()
	}
	
	$trustedIssuerNode = $doc.get_DocumentElement()."microsoft.identityModel".service.issuerNameRegistry.trustedIssuers.add
	if($trustedIssuerNode -ne $null){
		$trustedIssuerNode.thumbprint = $adfsCertificate.Thumbprint
	}
	
	$certificateReferenceNode = $doc.get_DocumentElement()."microsoft.identityModel".service.serviceCertificate.certificateReference
	if($certificateReferenceNode -ne $null){
		if ($certificateReferenceNode.x509FindType -eq "FindByThumbprint"){
			$certificateReferenceNode.findValue = $adfsCertificate.Thumbprint
		} else {
			$certificateReferenceNode.findValue = $certificateSubject
		}
	}
	
   	$doc.Save($webConfig)
}


Function BuildAdfsUrl($hostName){
	return (BuildWebSiteUrl $hostName 443 $true)+'/adfs/ls'
}

Function EncryptWebConfigSections($configSections, $physicalSitePath){
	$phiscalWebAppDir = $physicalSitePath.TrimEnd('/', '\')

	Write-Host
	Write-Host "Encrypting sections in config files:" -ForegroundColor Yellow
	
	foreach ($configSection in $configSections){
		Write-Host "	"$configSection
		$aspnet_regiis = GetAspnetRegiisLocation
		.$aspnet_regiis -pef $configSection $phiscalWebAppDir -prov "RsaProtectedConfigurationProvider" 
		CheckForErrors
	}
}

Function GetAspnetRegiisLocation(){
	if(Test-Path "$env:WINDIR\Microsoft.NET\Framework64\v4.0.30319"){
		return "$env:WINDIR\Microsoft.NET\Framework64\v4.0.30319\aspnet_regiis.exe"
	}
	return "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe"
}