[CmdletBinding()]
Param(
  [Parameter()] [string] $NuGetDir = $null,
  [Parameter()] [string] $MsBuildDir = $null,
  [Parameter()] [string] $DotCoverDir = $null,
  
  [Parameter()] [string] $Configuration = "Debug",
  [Parameter()] [string] $Targets = "Rebuild",
 
  [Parameter()] [bool] $SkipTests = $false
)

Set-StrictMode -Version 2.0; $ErrorActionPreference = "Stop"; $ConfirmPreference = "None"; trap { $host.SetShouldExit(1) }
Import-Module $PSScriptRoot\_Import.ps1

& $PSScriptRoot\CleanOutput.ps1
& $PSScriptRoot\Compile.ps1 -NuGetDir $NuGetDir -MsBuildDir $MsBuildDir -Configuration $Configuration -Targets $Targets
& $PSScriptRoot\TestAndCover.ps1 -DotCoverDir $DotCoverDir -Configuration $Configuration -SkipTests $SkipTests
& $PSScriptRoot\Package.ps1 -NuGetDir $NuGetDir -Configuration $Configuration
& $PSScriptRoot\Inspect.ps1 -Configuration $Configuration