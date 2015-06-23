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

if ($SkipTests) { exit }

Write-Step "Test And Cover"

function Cover ([string] $executable, [string] $arguments, [string] $output) {
  Exec { & $DotCover cover $CoverageConfig /TargetExecutable=$executable /TargetArguments=$arguments /Output=$output }
  Write-TeamCityImport "dotNetCoverage" $output "tool='dotcover'"  
}

# Variables
$TestResultFile     = Join-Path $OutputDir "TestResults.xml"
$CoverageResultFile = Join-Path $OutputDir "CoverageResults.dcvr"

# NUnit
$NUnitToolsDir = Join-Path (Get-SolutionPackagePath "NUnit.Runners") "tools"
$NUnit         = Join-Path $NUnitToolsDir "nunit-console-x86.exe"
$NUnitArgs     = $TestAssemblies + @("/noshadow", "/result=$TestResultFile")
Write-TeamCityImport "nunit" $TestResultFile
# Exec { & $NUnit $NUnitArgs }

Cover $NUnit $NUnitArgs $CoverageResultFile

