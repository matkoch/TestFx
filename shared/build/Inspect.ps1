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

Write-Step "Inspect Code"

# Variables
$InspectionResultFile = Join-Path $OutputDir "InspectionResults.xml"
$ToolsDir             = Join-Path (Get-SolutionPackagePath "ReSharperCommandLineTools") "tools"
$InspectCode          = Join-Path $ToolsDir "inspectcode.exe"
$InspectionExtensions = "ReSharper.ImplicitNullability;ReSharper.SerializationInspections;ReSharper.XmlDocInspections"
$InspectionCache      = Join-Path $SolutionDir "_ReSharper.InspectionCache"

$CodeAnalysisFiles    = gci $SourceDir -force -recurse -filter *.CodeAnalysisLog.xml

# ReSharper Inspections
Write-TeamCityImport "ReSharperInspectCode" $InspectionResultFile
#Exec { & $InspectCode @($SolutionFile, "/x=$InspectionExtensions", "/caches-home=$InspectionCache", "/o=$ResultFile") }

# FxCop
$CodeAnalysisFiles | %{
  Write-TeamCityImport "FxCop" $_
  }