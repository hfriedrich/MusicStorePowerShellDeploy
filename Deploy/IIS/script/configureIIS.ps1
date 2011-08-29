Function UnlockConfigurationSection($configSections){
	Write-Host
	Write-Host "Unlocking IIS configuration sections:" -ForegroundColor Blue
	
	foreach ($configSection in $configSections){
		Write-Host "	"$configSection
		
		set-webconfiguration //$configSection machine/webroot/apphost -metadata overrideMode -value Allow
		CheckForErrors
	}
}

Function RegisterAspNetV4WithIis(){
	Push-Location
	
	Write-Host
	Write-Host "Registering ASP.Net v4 with IIS:" -ForegroundColor Blue
	
	$pathToDotNetV4 = GetPathToDotNetV4
	if($pathToDotNetV4 -eq $null){
		Write-Host "FAILED! STOPPING SCRIPT EXECUTION" -foregroundcolor red
		Write-Host "Required .NET-Framework v.4 could not be found." -foregroundcolor red
    	exit
	}
	Set-Location $pathToDotNetV4
	.\aspnet_regiis.exe -iru
	CheckForErrors
	
	Pop-Location
}

Function GetPathToDotNetV4(){
	if(Test-Path $Env:windir\Microsoft.NET\Framework64){
		return ls $Env:windir\Microsoft.NET\Framework64 | ? { ($_.PSIsContainer) -and ($_.Name -match '^?4.0.?') } | select -exp FullName -l 1
	}
	return ls $Env:windir\Microsoft.NET\Framework | ? { ($_.PSIsContainer) -and ($_.Name -match '^?4.0.?') } | sort Name -des | select -exp FullName -l 1
}
