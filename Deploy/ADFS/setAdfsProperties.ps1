Function SetAdfsProperties($hostName){
	Set-ADFSProperties -HostName $hostName
	CheckForErrors
}