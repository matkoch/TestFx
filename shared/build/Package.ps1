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

Write-Step "Package"

$NuSpecFiles | %{
  Exec { & $NuGet pack $_ -Version $SemVer -Properties Configuration=$Configuration -BasePath $SourceDir -OutputDirectory $OutputDir -Symbols -NoPackageAnalysis }
}