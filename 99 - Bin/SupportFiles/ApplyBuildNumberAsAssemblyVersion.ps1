Param(
  [string]$pathToSearch = $env:BUILD_SOURCESDIRECTORY,
  [string]$buildNumber = $env:BUILD_BUILDNUMBER,
  [string]$buildNumberFormat = "(\d+)\.(\d+)\.(\d+)(?:-(.*))?",
  [string]$searchPattern = "**\AssemblyInfo.cs",
  [string]$buildNumberPattern,
  [string]$assemblyVersionReplacementPattern = '$1.$2.0.0',
  [string]$fileVersionReplacementPattern = '$1.$2.$3.$4',
  [string]$informationalVersionReplacementPattern = '$0'
)

$ErrorActionPreference = "Stop"

# Required for the Find-Files cmdlet.
Write-Verbose "Importing modules"

# The module dir contains the required TFS modules.
$moduleDir = "$env:AGENT_HOMEDIRECTORY\agent\Worker"
Write-Output "Using moduleDir ""$moduleDir""."

Import-Module "$moduleDir\Microsoft.TeamFoundation.DistributedTask.Agent.Interfaces.dll"
Import-Module "$moduleDir\Modules\Microsoft.TeamFoundation.DistributedTask.Task.Internal\Microsoft.TeamFoundation.DistributedTask.Task.Internal.dll"
Import-Module "$moduleDir\Modules\Microsoft.TeamFoundation.DistributedTask.Task.Common\Microsoft.TeamFoundation.DistributedTask.Task.Common.dll"
Import-Module "$moduleDir\Microsoft.TeamFoundation.DistributedTask.Agent.Common.dll"
Import-Module "$moduleDir\Microsoft.TeamFoundation.DistributedTask.Agent.Strings.dll"

# If an explicit Build number pattern is specified, use that. Otherwise use the selected format.
if($buildNumberPattern) {
    $pattern = $buildNumberPattern
} else {
    $pattern = $buildNumberFormat
}

Write-Verbose "Capturing build number parts from ""$buildNumber"" using pattern ""$pattern""..."
if ($buildNumber -match $pattern -ne $true) {
    Write-Error "Could not extract a version from [$buildNumber] using pattern [$pattern]"
    exit 2
}

Write-Verbose "Capture results: Major: ""$($Matches[1])"", Minor: ""$($Matches[2])"", Patch: ""$($Matches[3])"", Prerelease/Revision: ""$($Matches[4])""."

# Generate the different versions based on the $buildNumber. 
# Clean up the assembly and file versions by removing any characters other than digits and '.', because they're not allowed here.
$assemblyVersion = (($buildNumber -replace $pattern, $assemblyVersionReplacementPattern) -replace '[^\d\.]', '')
$fileVersion = (($buildNumber -replace $pattern, $fileVersionReplacementPattern) -replace '[^\d\.]', '')
$informationalVersion = ($buildNumber -replace $pattern, $informationalVersionReplacementPattern)
Write-Host "Using version ""$assemblyVersion"", file version ""$fileVersion"" and informational version ""$informationalVersion""."



# Declare functions
function Replace-Version($content, $version, $attribute) {
    $pattern = '\[assembly: {0}(?:Attribute)?\(".*"\)\]' -f @($attribute)
    $replacement = ('[assembly: {0}("{1}")]' -f @($attribute, $version))

    $versionReplaced = $false
    $content = $content | %{
        if ($_ -match $pattern) {
            $versionReplaced = $true
            Write-Host "     * Replaced $($Matches[0]) with $replacement"
            $_ = $_ -replace [regex]::Escape($Matches[0]),$replacement
        }
        $_
    }
    if (-not $versionReplaced) {
        Write-Host "     * Added $replacement to end of content"
        $content += [System.Environment]::NewLine + $replacement
    }
    return $content
}

function Get-AssemblyInfoFiles {
    [cmdletbinding()]
    param([string]$searchPattern)

    # check for solution pattern
    if ($searchPattern.Contains("*") -or $searchPattern.Contains("?") -or $searchPattern.Contains(";"))
    {
        Write-Verbose "Pattern found in solution parameter."    
        if ($env:BUILD_SOURCESDIRECTORY)
        {
            Write-Verbose "Using build.sourcesdirectory as root folder"
            Write-Host "Find-Files -SearchPattern $searchPattern -RootFolder $env:BUILD_SOURCESDIRECTORY"
            $foundFiles = Find-Files -SearchPattern $searchPattern -RootFolder $env:BUILD_SOURCESDIRECTORY
        }
        elseif ($env:SYSTEM_ARTIFACTSDIRECTORY)
        {
            Write-Verbose "Using system.artifactsdirectory as root folder"
            Write-Host "Find-Files -SearchPattern $searchPattern -RootFolder $env:SYSTEM_ARTIFACTSDIRECTORY"
            $foundFiles = Find-Files -SearchPattern $searchPattern -RootFolder $env:SYSTEM_ARTIFACTSDIRECTORY
        }
        else
        {
            Write-Host "Find-Files -SearchPattern $searchPattern"
            $foundFiles = Find-Files -SearchPattern $searchPattern
        }
    }
    else
    {
        Write-Verbose "No pattern found in solution parameter."
        $foundFiles = ,$searchPattern
    }
    $foundFiles
}

foreach($assemblyInfoFile in Get-AssemblyInfoFiles -SearchPattern $searchPattern)
{
    Write-Host "  -> Changing $($assemblyInfoFile)"
         
    # remove the read-only bit on the file
    Set-ItemProperty $assemblyInfoFile IsReadOnly $false
 
    # run the regex replace
    $content = Get-Content $assemblyInfoFile
    $content = Replace-Version -content $content -version $assemblyVersion -attribute 'AssemblyVersion'
    $content = Replace-Version -content $content -version $fileVersion -attribute 'AssemblyFileVersion'
    $content = Replace-Version -content $content -version $informationalVersion -attribute 'AssemblyInformationalVersion'
    $content | Set-Content $assemblyInfoFile -Encoding UTF8
}


Write-Host "Done!"