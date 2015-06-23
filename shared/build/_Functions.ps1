
function ??($a, $b) { if ($a -ne $null) { $a } else { $b } }

function IsNullOrEmpty ([string] $value) { [string]::IsNullOrEmpty($value) }

function Get-ScriptDirectory {
  $Invocation = (Get-Variable MyInvocation -Scope 1).Value
  Split-Path $Invocation.MyCommand.Path
}

function Get-GitSemVer {
  Import-Module (Join-Path $PSScriptRoot "\..\tools\GetGitInfo.dll")
  Get-Version $SolutionDir
}

function Get-SolutionPackagePath([string] $packageId) {
  $SolutionPackagesConfig = Join-Path $SolutionDir ".nuget\packages.config"
  
  [xml] $xml = Get-Content $SolutionPackagesConfig
  $version = $xml.SelectNodes("/packages/package[@id = '$packageId']/@version") | Select -ExpandProperty Value
  return Join-Path (Join-Path $SolutionDir "packages") "$packageId.$version"
}

function Exec([scriptblock] $cmd) {
  & $cmd
  if ($LastExitCode -ne 0) {
    throw "'$cmd' exited with $LastExitCode."
  }
}

function Write-Step ([string] $step) {
  Write-Host -ForegroundColor "magenta" "`n$step`n"
  Write-TeamCity "progressMessage '$step']"
  
  $Host.UI.RawUI.WindowTitle = "$step ($SemVer)"
  
  Write-TeamCity "buildNumber '$SemVer'"
}

function Write-TeamCityImport ([string] $type, [string] $path, [string] $additional = "") {
  Write-TeamCity "importData type='$type' path='$path' verbose='true' whenNoDataPublished='error' $additional"
}

function Write-TeamCity ([string] $message) {
  Write-Host "##teamcity[$message]"
}