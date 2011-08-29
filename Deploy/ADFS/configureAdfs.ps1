param 
( 
	[parameter(Mandatory=$true, HelpMessage="Name of ADFS-host")]
	[ValidateNotNullOrEmpty()]
	$adfsHostName,

	[parameter(Mandatory=$true, HelpMessage="Name of the web application site to trust")]
	[ValidateNotNullOrEmpty()]
	$relyingPartyTrustName,
	
	[parameter(Mandatory=$true, HelpMessage="URI of the web application site to trust")]
	[ValidateNotNullOrEmpty()]
	$relyingPartyTrustIdentifier,
	
	[parameter(Mandatory=$false, HelpMessage="Path to the web application site's certificate file")]
	[ValidateNotNullOrEmpty()]
	$relyingPartyTrustCertifateFileLocation,
	
	[parameter(Mandatory=$true, HelpMessage="Subject of the certificate for securing the communication")]
	[ValidateNotNullOrEmpty()]
	$communicationCertificateSubject,
	
	[parameter(Mandatory=$true, HelpMessage="Subject of the certificate for decrypting the tokens")]
	[ValidateNotNullOrEmpty()]
	$tokenDecryptingCertificateSubject,
	
	[parameter(Mandatory=$true, HelpMessage="Subject of the certificate for signing the tokens")]
	[ValidateNotNullOrEmpty()]
	$tokenSigningCertificateSubject
)

. .\setAdfsProperties.ps1
. .\setCertificates.ps1
. .\addRelyingPartyTrust.ps1

$AdfsLocation = "$env:ProgramFiles\Active Directory Federation Services 2.0"


$adfsSnapin = Get-PSSnapin | where {$_.Name -eq 'Microsoft.Adfs.PowerShell'}
if($adfsSnapin -eq $null){
	Add-PSSnapin Microsoft.Adfs.PowerShell
}

Function CheckForErrors() {
  if (!$?) {
  	Write-Host "--------------------------" -foregroundcolor red
	Write-Host "FAILED! STOPPING EXECUTION" -foregroundcolor red
    exit
  }
}

[System.Console]::BackgroundColor = 'DarkBlue'
[System.Console]::ForegroundColor = 'Gray'
Clear-Host

Write-Host "Starting ADFS configuration"
Write-Host "---------------------------"

SetAdfsProperties $adfsHostName
SetCertificates $communicationCertificateSubject $tokenDecryptingCertificateSubject $tokenSigningCertificateSubject
AddRelyingPartyTrust $relyingPartyTrustName $relyingPartyTrustIdentifier $relyingPartyTrustCertifateFileLocation

Write-Host
Write-Host "Restarting ADFS-Service"
Restart-Service -displayname "AD FS 2.0 Windows Service"
CheckForErrors

Write-Host
Write-Host "-------------------------------------" -foregroundcolor green
Write-Host "ADFS has been configured successfully" -foregroundcolor green
Write-Host