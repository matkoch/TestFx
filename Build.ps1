[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True)] [string] $BuildRunner,
  [Parameter(Mandatory=$True)] [string] $MsBuildDir,
  [Parameter(Mandatory=$True)] [string] $NuGetDir,
  [Parameter()] [string] $DotCoverDir,
  
  [Parameter()] [string] $Configuration = "Debug",
  [Parameter()] [string] $Targets = "Rebuild",
 
  [Parameter()] [bool] $SkipTests = $false,
  [Parameter()] [bool] $RunDotCover = $false,
  
  [Parameter()] [string] $NuGetKey,
  [Parameter()] [string] $ReSharperKey
)

# Preamble
Set-StrictMode -Version 2.0; $ErrorActionPreference = "Stop"; $ConfirmPreference = "None"; trap { $host.SetShouldExit(1) }

# User variables
$SolutionFile     = "TestFx.sln"
$SourceDir        = "src"
$NuSpecDir        = "nuspec"
$OutputDir        = "output"

$CoverageFile     = Join-Path $SourceDir "coverage.xml"
$AssemblyInfoFile = Join-Path $SourceDir "AssemblyInfoShared.cs"

[array] `
$TestAssemblies   = @("TestFx.Specifications.IntegrationTests") | %{
                      Join-Path $SourceDir "$_\bin\$Configuration\$_.dll"
                     }

# Imports
. .\Build.Functions.ps1
. .\Build.Variables.ps1

if ($BuildRunner -eq "TeamCity") { Write-Host "##teamcity[buildNumber '$SemVer']" }

# Tasks
Clean
Restore
Build
Test
Pack

