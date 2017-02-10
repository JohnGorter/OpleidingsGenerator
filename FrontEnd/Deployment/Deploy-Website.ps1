param (    
    [Parameter(Mandatory=$True)]
    [string]$backendAddress,
       
    [Parameter(Mandatory=$True)]
    [string]$appPoolName

)

# Let failures throw an exception so that we can catch it.
$ErrorActionPreference = "Stop"

cd $PSScriptRoot

try
{
    $sourceDir = [System.IO.Path]::GetFullPath("$pwd\..")
    $targetDir = "D:\applicaties\OpleidingsplanGenerator\Frontend"
    $backupBaseDir = "D:\Deployment\Pre-deployment backups\OpleidingsplanGenerator\Frontend"

    Write-Output "Current working directory:      $pwd"
    Write-Output "Source directory:               $sourceDir"
    Write-Output "Target directory:               $targetDir"
    Write-Output "Backup directory:               $backupBaseDir"
    Write-Output "Current identity:               $env:USERDOMAIN\$env:USERNAME"
    Write-Output "Parameters:"
    Write-Output "  BackendAddress:               $backendAddress"
    Write-Output "  AppPoolName:                  $appPoolName"
    											  
    Write-Output "################ Backup started!"
        # Backup project
        $backupDir = mkdir ("{0}\OpleidingsplanGenerator Frontend {1:yyyyMMdd_HHmmss}" -f @($backupBaseDir, [System.DateTime]::Now))

        # Archive the application folder; it contains everything.
        Write-Output "Copying $targetDir..."
        Copy-Item "$targetDir\*" $backupDir -Recurse

        # Package everything
        Write-Output "Zipping everything to $backupDir.zip..."
        Add-Type -assembly "system.io.compression.filesystem"
        [io.compression.zipfile]::CreateFromDirectory($backupDir, "$backupDir.zip")

        # Now that we have the zip, delete the backup direcory...
        Write-Output "Removing $backupDir..."
        Remove-Item $backupDir -Recurse -Force
    Write-Output "################ Backup done!"


    
    Write-Output "################ Release started!"
        # Stop the app-pool and give IIS the time to release the assemblies
        Write-Output "Stopping the $appPoolName in order to release the website assemblies..."
        try { Stop-WebAppPool $appPoolName } catch { }
        Start-Sleep -s 5

        # Remove the files twice - sometimes the first time fails with a "directory is not empty"
        if(Test-Path $targetDir)
        {
            Write-Output "Removing old files..."
            Get-ChildItem $targetDir | Where-Object {$_.Name -ne "App_Data" } | Remove-Item -Recurse -Force
            Get-ChildItem $targetDir | Where-Object {$_.Name -ne "App_Data" } | Remove-Item -Recurse -Force
        }
        else
        {
            Write-Output "Creating directory ""$targetDir""..."
            #[void](md $targetDir)
        }

        Write-Output "Copying files to ""$targetDir""..."
        Get-ChildItem $sourceDir | Where-Object {$_.Name -ne "Deployment" } | Copy-Item -Destination $targetDir -Recurse -Force -Verbose 4>&1 | Out-String -Width 4096 | Write-Output

        Write-Output "Replacing the web.config placeholders..."
        $config = Get-Content "$targetDir\Web.config" -Raw
        $config = $config -replace "__BackendAddress__", $backendAddress
        $config | Set-Content "$targetDir\Web.config"

        Write-Output "Starting the $appPoolName again..."
		Start-WebAppPool $appPoolName
    Write-Output "################ Release done!"
}
catch
{
    # Powershell tracks all Exceptions that occured so far in $Error
	Write-Output "$Error"

	# Signal failure to MSRM:
	$ErrorActionPreference = "Continue"
	Write-Error "Error: $Error"
}