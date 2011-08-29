Function GetCertirficateFromCertificateProvider($certificateSubject){
	Push-Location
	cd cert:
	$allCertificates = ls .\LocalMachine\My
	$certificate = $allCertificates | where {$_.Subject.StartsWith('CN='+$certificateSubject) }	
	Pop-Location
	return $certificate
}

Function SetPrimaryTokenCertificate($certificate, $certificateType){
	$adfsCertificate = Get-ADFSCertificate -Thumbprint $certificate.thumbprint | where {$_.CertificateType -eq $certificateType}
	if($adfsCertificate -eq $null){
		Write-Host "	Subject: "$communicationCertificate.Subject", Type: "$certificateType  -ForegroundColor DarkGray
		Add-ADFSCertificate -CertificateType $certificateType –Thumbprint $certificate.thumbprint -IsPrimary		
		CheckForErrors
	}
}

Function SetCertificates($communicationCertificateSubject, $tokenDecryptingCertificateSubject, $tokenSigningCertificateSubject){
	$communicationCertificate = GetCertirficateFromCertificateProvider $communicationCertificateSubject
	$tokenDecryptingCertificate = GetCertirficateFromCertificateProvider $tokenDecryptingCertificateSubject
	$tokenSigningCertificate = GetCertirficateFromCertificateProvider $tokenSigningCertificateSubject
	
	Write-Host
	Write-Host "Setting Certificates:"
	
	Write-Host "	Disabling automatic certificate rollover"  -ForegroundColor DarkGray
	Set-AdfsProperties -AutoCertificateRollover $false
	CheckForErrors
	
	Write-Host "	Subject: "$communicationCertificate.Subject", Type: Service communications"  -ForegroundColor DarkGray
	
	Set-ADFSCertificate -CertificateType 'Service-Communications' -Thumbprint $communicationCertificate.thumbprint
	CheckForErrors
	SetPrimaryTokenCertificate $tokenDecryptingCertificate 'Token-Decrypting'
	SetPrimaryTokenCertificate $tokenSigningCertificate 'Token-Signing'
}