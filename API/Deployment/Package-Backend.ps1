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
$opleidingsplanBackendSourceDir = "$SourceBaseDir\API"
    
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

$opleidingsplanBackendTargetDir = "$TargetDir\opleidingsplanBackend"
Write-Output "Clear the targetDir."
CreateDirectory $opleidingsplanBackendTargetDir $Force
Remove-Item "$opleidingsplanBackendTargetDir\*" -Recurse


Write-Output "Copying the BackendService files to ($opleidingsplanBackendTargetDir)"
Get-ChildItem "$opleidingsplanBackendSourceDir\_PublishedWebsites\*"
Copy-Item -Path "$opleidingsplanBackendSourceDir\_PublishedWebsites\*" -Destination $opleidingsplanBackendTargetDir -Recurse
Get-ChildItem -Path $opleidingsplanBackendTargetDir -Recurse | Where-Object { !$_.PSIsContainer } | foreach { $_.IsReadOnly = $false }
Get-ChildItem -Path $opleidingsplanBackendTargetDir -Include $buildStuff -Recurse | Remove-Item
Get-ChildItem -Path $opleidingsplanBackendTargetDir -Filter "_PublishedWebsites" -Recurse | Remove-Item -Recurse
Get-ChildItem -Path $opleidingsplanBackendTargetDir -Filter "App_Data" -Recurse | Remove-Item -Recurse

Write-Output "Clearing read-only attributes..."
Get-ChildItem -Path $TargetDir -Recurse | Where-Object { !$_.PSIsContainer } | foreach { $_.IsReadOnly = $false }

#
# Part 3: Creating Web.config template.
# 

Write-Output "Make a template for the Web.Config (opleidingsplanBackend)"
$configFilePath = "$opleidingsplanBackendTargetDir\Web.config"

$xml = New-Object System.Xml.XmlDocument
$xml.PreserveWhitespace = $true
$xml.Load($configFilePath)

$xml.SelectSingleNode("//configuration//generatorConfigurations/@education-plan-file-dir-path").value = "__PlanFilePath__"

$xml.SelectSingleNode("//configuration//DalJsonConnection/@profile-path").value = "__ProfilePath__"
$xml.SelectSingleNode("//configuration//DalJsonConnection/@educationplan-path").value = "__EducationplanPath__"
$xml.SelectSingleNode("//configuration//DalJsonConnection/@management-properties-path").value = "__ManagementPropertiesPath__"
$xml.SelectSingleNode("//configuration//DalJsonConnection/@educationplan-updated-path").value = "__EducationPlanUpdatedPath__"
$xml.SelectSingleNode("//configuration//DalJsonConnection/@module-path").value = "__ModulePath__"

$xml.SelectSingleNode("//configuration//serviceConnection/@info-support-training-url").value = "__TrainingUrl__"


$xml.Save($configFilePath)