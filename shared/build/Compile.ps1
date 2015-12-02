[CmdletBinding()]
Param(
  [Parameter()] [string] $NuGetDir = $null,
  [Parameter()] [string] $MsBuildDir = $null,
  [Parameter()] [string] $DotCoverDir = $null,
  
  [Parameter()] [string] $Configuration = "Release",
  [Parameter()] [string] $Targets = "Rebuild",
 
  [Parameter()] [bool] $SkipTests = $false
)

Set-StrictMode -Version 2.0; $ErrorActionPreference = "Stop"; $ConfirmPreference = "None"; trap { $host.SetShouldExit(1) }
Import-Module $PSScriptRoot\_Import.ps1

Write-Step "Build"

# Variables
$AssemblyInfoFile = Join-Path $SolutionDir $AssemblyInfoFile
$BackupFile       = $AssemblyInfoFile + ".bkp"

# Backup AssemblyInfo
if (-not (Test-Path $BackupFile)) { cp $AssemblyInfoFile $BackupFile }

# Update AssemblyInfo
(Get-Content $AssemblyInfoFile) | Foreach-Object {
  $_ -replace 'AssemblyConfiguration\s*\(".+"\)', ('AssemblyConfiguration ("' + $Configuration.ToUpper() + '")') `
     -replace 'AssemblyVersion\s*\(".+"\)', ('AssemblyVersion ("' + $SemVer.Major + '.0.0.0")') `
     -replace 'AssemblyFileVersion\s*\(".+"\)', ('AssemblyFileVersion ("' + $SemVer.Major + '.' + $SemVer.Minor + '.' + $SemVer.Patch + '.0")') `
     -replace 'AssemblyInformationalVersion\s*\(".+"\)', ('AssemblyInformationalVersion ("' + $SemVer + ' [' + $GitInfo.Branch + '] (Sha: ' + $GitInfo.Commit + ')")')
  } | sc $AssemblyInfoFile

try {
  New-Item -ItemType Directory -Force -Path $OutputDir | out-null
  
  # Restore packages
  Exec { & $NuGet @("restore", $SolutionFile, "-NonInteractive") }
  
  # Compile project
  Exec { & $MsBuild @($SolutionFile, "/t:$Targets", "/p:Configuration=$Configuration;Platform=Any CPU", "/m", "/nr:false", `
               "/fl", "/flp:LogFile=$OutputDir\MsBuild.log;Verbosity=diag") }
}
finally {
  # Revert AssemblyInfo backups
  mv $BackupFile $AssemblyInfoFile -force
}