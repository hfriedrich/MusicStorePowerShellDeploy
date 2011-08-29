$adfsHostName = 'adfs.bekk.Fagdag2011.no'
$relyingPartyTrustName = 'MvcMusicStoreTest'
#$relyingPartyTrustIdentifier = 'https://web.bekk.Fagdag2011.no:83'
$relyingPartyTrustIdentifier = Read-Host "Enter the application's URL"
$adfsCertificateSubject = 'adfs.bekk.Fagdag2011.no'

$command = '.\configureAdfs.ps1 -adfsHostName $adfsHostName -relyingPartyTrustName $relyingPartyTrustName -relyingPartyTrustIdentifier $relyingPartyTrustIdentifier -communicationCertificateSubject $adfsCertificateSubject -tokenDecryptingCertificateSubject $adfsCertificateSubject -tokenSigningCertificateSubject $adfsCertificateSubject'
Invoke-Expression $command