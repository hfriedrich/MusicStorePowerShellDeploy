$adfsHostName = 'WIN-IA6KB8UL431.da.domain.test'
$relyingPartyTrustName = 'MvcMusicStoreTest'
$relyingPartyTrustIdentifier = 'https://win-ia6kb8ul431.da.domain.test:83'
$adfsCertificateSubject = 'WIN-IA6KB8UL431.da.domain.test'

$command = '.\configureAdfs.ps1 -adfsHostName $adfsHostName -relyingPartyTrustName $relyingPartyTrustName -relyingPartyTrustIdentifier $relyingPartyTrustIdentifier -communicationCertificateSubject $adfsCertificateSubject -tokenDecryptingCertificateSubject $adfsCertificateSubject -tokenSigningCertificateSubject $adfsCertificateSubject'
Invoke-Expression $command