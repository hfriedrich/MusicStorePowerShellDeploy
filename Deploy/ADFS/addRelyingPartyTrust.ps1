Function AddRelyingPartyTrust($partyTrustName, $partyTrustIdentifier, $partyTrustCertifateFileLocation){
	$rules += GetPassthroughNameRule
	$ruleSet = New-ADFSClaimRuleSet -ClaimRule $rules 
	
	$issuanceAuthorizationRule = (GetAuthorizationPermissionRule).ToString();
	
	Write-Host
	Write-Host "Adding Relying Party Trust:"
	Write-Host "	Name: "$partyTrustName", ID: "$partyTrustIdentifier  -ForegroundColor DarkGray
	
	if((Get-ADFSRelyingPartyTrust -Name $partyTrustName) -ne $null){
		Remove-ADFSRelyingPartyTrust -TargetName $partyTrustName
		CheckForErrors
	}

	if($partyTrustCertifateFileLocation -ne $null){
		$certificate = New-Object System.Security.Cryptography.X509Certificates.X509Certificate (Get-Item -Path $partyTrustCertifateFileLocation).FullName
		Add-ADFSRelyingPartyTrust -Name $partyTrustName -Enabled $true -Identifier $partyTrustIdentifier -EncryptionCertificate $certificate -WSFedEndpoint $wsEndpoint -IssuanceAuthorizationRules $issuanceAuthorizationRule -DelegationAuthorizationRules $delegationAuthorizationRule -IssuanceTransformRules $ruleSet.ClaimRulesString
	}else{
		Add-ADFSRelyingPartyTrust -Name $partyTrustName -Enabled $true -Identifier $partyTrustIdentifier -WSFedEndpoint $partyTrustIdentifier -IssuanceAuthorizationRules $issuanceAuthorizationRule -DelegationAuthorizationRules $delegationAuthorizationRule -IssuanceTransformRules $ruleSet.ClaimRulesString
	}

	CheckForErrors
}

Function GetPassthroughNameRule(){
	return '@RuleTemplate = "PassThroughClaims" @RuleName = "Pass through name" c:[Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] => issue(claim = c);'
}

Function GetAuthorizationPermissionRule(){
	return '=> issue(Type = "http://schemas.microsoft.com/authorization/claims/permit", Value = "true");'	
}