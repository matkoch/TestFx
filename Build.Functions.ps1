
function ??($a, $b) { if ($a -ne $null) { $a } else { $b } }

function Get-ScriptDirectory {
  $Invocation = (Get-Variable MyInvocation -Scope 1).Value
  Split-Path $Invocation.MyCommand.Path
}

function Get-GitSemVer {
  Import-Module (Join-Path $ToolsDir "GetGitInfo.dll")
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

function Clean() {
  Write-Step "Clean"
  New-Item $OutputDir -Type Directory -Force | Out-Null
  Remove-Item $OutputDir\* -Recurse -Force
}

function Restore() {
  Write-Step "Restore"
  $NuGet = Join-Path (Resolve-Path $NuGetDir) $NuGetExe
  Exec { & $NuGet restore $SolutionFile }
}

function Build() {
  Write-Step "Build"

  $MsBuild          = Join-Path (Resolve-Path $MsBuildDir) $MsBuildExe
  $AssemblyInfoFile = Join-Path $SolutionDir $AssemblyInfoFile
  $BackupFile       = $AssemblyInfoFile + ".bkp"
  
  if (-not (Test-Path $BackupFile)) { cp $AssemblyInfoFile $BackupFile }

  (Get-Content $AssemblyInfoFile) | Foreach-Object {
    $_ -replace 'AssemblyConfiguration\s*\(".+"\)', ('AssemblyConfiguration ("' + $Configuration.ToUpper() + '")') `
       -replace 'AssemblyVersion\s*\(".+"\)', ('AssemblyVersion ("' + $SemVer.Major + '.0.0.0")') `
       -replace 'AssemblyFileVersion\s*\(".+"\)', ('AssemblyFileVersion ("' + $SemVer.Major + '.' + $SemVer.Minor + '.' + $SemVer.Patch + '.0")') `
       -replace 'AssemblyInformationalVersion\s*\(".+"\)', ('AssemblyInformationalVersion ("' + $SemVer + ' [' + $GitInfo.Branch + '] (Sha: ' + $GitInfo.Commit + ')")')
    } | sc $AssemblyInfoFile

  try {
    New-Item -ItemType Directory -Force -Path $OutputDir | out-null
    Exec { & $NuGet @("restore", $SolutionFile, "-NonInteractive") }
    Exec { & $MsBuild @($SolutionFile, "/t:$Targets", "/p:Configuration=$Configuration;Platform=Any CPU", "/m", "/nr:false", `
                 "/fl", "/flp:LogFile=$OutputDir\MsBuild.log;Verbosity=diag") }
  }
  finally {
    mv $BackupFile $AssemblyInfoFile -force
  }
}

function Test() {
  Write-Step "Test"
  
  $TestResultFile     = Join-Path $OutputDir "TestResults.xml"
  $CoverageResultFile = Join-Path $OutputDir "CoverageResults.dcvr"
  
  $NUnitToolsDir = Join-Path (Get-SolutionPackagePath "NUnit.Runners") "tools"
  $NUnit         = Join-Path $NUnitToolsDir $NUnitExe
  $NUnitArgs     = $TestAssemblies + @("/noshadow", "/result=$TestResultFile")
  Write-TeamCityImport "nunit" $TestResultFile
  
  Cover $NUnit $NUnitArgs $CoverageResultFile
}

function Cover ([string] $executable, [string] $arguments, [string] $output) {
  $DotCover = Join-Path $DotCoverDir $DotCoverExe
  
  Exec { & $DotCover cover $CoverageFile /TargetExecutable=$executable /TargetArguments=$arguments /Output=$output }
  Write-TeamCityImport "dotNetCoverage" $output "tool='dotcover'"  
}

function Inspect () {
  Write-Step "Inspect"
  Write-TeamCityImport "ReSharperInspectCode" "Build\ReSharperInspectCodeOutput.xml"
}

function Pack() {
  Write-Step "Pack"
  
  $NuGet       = Join-Path (Resolve-Path $NuGetDir) $NuGetExe
  $NuSpecFiles = gci $NuSpecDir -force -recurse -filter *.nuspec

  $NuSpecFiles | %{
    Exec { & $NuGet pack $_.FullName -Version $SemVer -Properties "Id=$($_.BaseName);Configuration=$Configuration" -BasePath $SourceDir -OutputDirectory $OutputDir -Symbols -NoPackageAnalysis }
  }
}

function Write-Step ([string] $step) {
  Write-Host -ForegroundColor "magenta" "`n$step`n"
  Write-TeamCity "progressMessage '$step']"
  Write-ConsoleTitle $step
}

function Write-TeamCityImport ([string] $type, [string] $path, [string] $additional = "") {
  Write-TeamCity "importData type='$type' path='$path' verbose='true' whenNoDataPublished='error' $additional"
}

function Write-TeamCity ([string] $message) {
  if ($BuildRunner -eq "TeamCity") { Write-Host "##teamcity[$message]" }
}

function Write-ConsoleTitle ([string] $message) {
  if ($BuildRunner -eq "Local") { $Host.UI.RawUI.WindowTitle = "$message ($SemVer)" }
}