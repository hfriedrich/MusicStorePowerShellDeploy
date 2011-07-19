Function ReadUserAccountNameFromConsole($message){
	return Read-Host $message
}

Function ReadPasswordFromConsole($message){
	return Read-Host $message -assecurestring
}

Function CreateLocalUserAccount($accountName, $password){
	if(ExistsUserAccount $accountName){
		return
	}
	
	Write-Host
	Write-Host "Creating new user account: "$accountName -ForegroundColor Yellow
	
	$user = (GetLocalComputerInfo).Create("User", $accountName) 
	$user.SetPassword((ConvertToPlainText $password))
	$user.UserFlags = 65536
 	$user.SetInfo()
}

Function ExistsUserAccount($accountName){
	$userNames = ((GetLocalComputerInfo).psbase.children |
	    Where-Object {$_.psBase.schemaClassName -eq "User"} |
	        Select-Object -expand Name)

	return $userNames -contains $accountName
}

Function GetLocalComputerInfo(){
	return [adsi] "WinNT://."
}

Function GrantModifyRightsOnSiteTo($accountName, $directory){
	Write-Host
	Write-Host "Granting modify-rights on "$directory.Trim() " to: "$accountName -ForegroundColor Yellow

	$acl = Get-Acl $directory
	$inherit = [system.security.accesscontrol.InheritanceFlags]"ContainerInherit, ObjectInherit"
	$propagation = [system.security.accesscontrol.PropagationFlags]::InheritOnly
	$modifyRight = [System.Security.AccessControl.FileSystemRights]::Modify
	$allowAccessType =[System.Security.AccessControl.AccessControlType]::Allow
	$allowModifyRule = New-Object system.security.accesscontrol.filesystemaccessrule($accountName, $modifyRight, $inherit, $propagation, $allowAccessType)
	$acl.SetAccessRule($allowModifyRule)
	Set-Acl $directory $acl
}