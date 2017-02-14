[CmdletBinding()]
param (
    # ...
    [string]$SourceBaseDir,
    
    # ...
    [string]$TargetDir,
    
    # By default, the packaged files are placed in a folder based on the version and build number (that is, the File 
    # version, i.e. "2.0.451.0"). Specify this to not create such a folder, but place the files in the TargetDir root.
    [switch]$DontCreateVersionFolder,
    
    # Unless specified, some values in the .config files are updated with Release Management config placeholders (such 
    # as "__CoressServiceUrl__"), so that the package can be used by RM.
    # [switch]$NoConfigPlaceholders,
    
    # If specified, overwrites the existing files that may exist. By default, the script aborts if any of the package 
    # folders already exist.
    [switch]$Force 
)

Write-Output "Starting the release!"
Write-Output "SourceDir: $SourceBaseDir"
Write-Output "TargetDir: $TargetDir"
Write-Output "DontCreateVersionFolder: $DontCreateVersionFolder"
Write-Output "NoConfigPlaceholders: $NoConfigPlaceholders"
Write-Output "Force: $Force"

function CreateDirectory([string]$targetDir, [bool]$force)
{
    if(Test-Path $targetDir)
    {
        if($force -eq $true)
        {
            Write-Verbose "The directory ""$targetDir"" already exists but -Force is specified, deleting it..."
            Remove-Item $TargetDir -Recurse
        }
        else
        {
            throw "The directory ""$targetDir"" already exists and -Force is not specified, cannot continue."
        }
    }

    Write-Verbose "Creating the directory ""$targetDir""..."
    [void](mkdir $targetDir)   
}

$ErrorActionPreference = "Stop"
# The files are taken from the \Deliverables folder.
$deliverablesSourceDir = "$SourceBaseDir\99 - Bin\Deliverables"
$opleidingsplanFrontendSourceDir = "$SourceBaseDir\FrontEnd"
    
Write-Verbose "Checking if the source and target directories are accessible or creating them."
@($deliverablesSourceDir, $TargetDir) | foreach {
    if(!(Test-Path $_))
    {
        CreateDirectory $_ $Force
    }
}


#
# Part 2: The actual copying.
# 

# Define the stuff we don't want in the package
$buildStuff = @("packages.config", "BuildInfo.config", "*.xml", "*.vshost.exe*", "*.lastcodeanalysissucceeded", "*unittest*")

$opleidingsplanFrontendTargetDir = "$TargetDir\opleidingsplanFrontend"
Write-Output "Clear the targetDir."
Remove-Item "$opleidingsplanFrontendTargetDir\*" -Recurse
CreateDirectory $opleidingsplanFrontendTargetDir $Force

Write-Output "Copying the BackendService files to ($opleidingsplanFrontendTargetDir)"
Get-ChildItem "$opleidingsplanFrontendSourceDir\_PublishedWebsites\*"
Copy-Item -Path "$opleidingsplanFrontendSourceDir\_PublishedWebsites\*" -Destination $opleidingsplanFrontendTargetDir -Recurse
Get-ChildItem -Path $opleidingsplanFrontendTargetDir -Recurse | Where-Object { !$_.PSIsContainer } | foreach { $_.IsReadOnly = $false }
Get-ChildItem -Path $opleidingsplanFrontendTargetDir -Include $buildStuff -Recurse | Remove-Item
Get-ChildItem -Path $opleidingsplanFrontendTargetDir -Filter "_PublishedWebsites" -Recurse | Remove-Item -Recurse

Write-Output "Clearing read-only attributes..."
Get-ChildItem -Path $TargetDir -Recurse | Where-Object { !$_.PSIsContainer } | foreach { $_.IsReadOnly = $false }

#
# Part 3: Creating Web.config template.
# 

Write-Output "Make a template for the Web.Config (opleidingsplanFrontend)"
$configFilePath = "$opleidingsplanFrontendTargetDir\Web.config"

$xml = New-Object System.Xml.XmlDocument
$xml.PreserveWhitespace = $true
$xml.Load($configFilePath)

$xml.SelectSingleNode("//configuration//appSettings/add[@key='BackendAdress']/@value").value = "__BackendAddress__"

$xml.Save($configFilePath)