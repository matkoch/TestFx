Set-StrictMode -Version 2.0
$ErrorActionPreference = "Stop"
trap { $host.SetShouldExit(1) }

function ??($a, $b) { if ($a -ne $null) { $a } else { $b } }
function Get-ScriptDirectory
{
  $Invocation = (Get-Variable MyInvocation -Scope 1).Value
  Split-Path $Invocation.MyCommand.Path
}

$SolutionDir   = ?? ${env:SourcePath}      (Get-ScriptDirectory)
$NuGet         = ?? ${env:NuGet}           (Join-Path $SolutionDir "..\NuGet.exe")
$Configuration = ?? ${env:Configuration}   "Debug"
$Platform      = ?? ${env:Platform}        "Any CPU"
$Targets       = ?? ${env:Targets}         "Rebuild"
$BuildRunner   = ?? ${env:BuildRunner}     "Local"

$OutputDir     = Join-Path $SolutionDir "output"
$NuspecdDir      = Join-Path $SolutionDir "nuspec"
$ToolsDir      = Join-Path $SolutionDir "tools"
$SourceDir     = Join-Path $SolutionDir "src"
$MsBuild       = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"

Import-Module (Join-Path $ToolsDir "GetGitInfo.dll")

$GitInfo = Get-Version $SolutionDir
$SemVer = $GitInfo.SemVer
$Host.UI.RawUI.WindowTitle = "Building $SemVer"

$AssemblyInfoFiles = gci $SourceDir -force -recurse -filter AssemblyInfo*.cs
$NuspecFiles = gci $NuspecdDir -force -recurse -filter *.nuspec
$SolutionFiles = gci -force $SolutionDir -filter *.sln

Write-Host "Backuping assembly info files"
$AssemblyInfoFiles | %{ $_.FullName } | ?{-not (Test-Path ($_ + ".bkp")) } | %{ cp $_ ($_ + ".bkp") }

Write-Host "Updating assembly info"
$AssemblyInfoFiles | %{
  gc ($_.FullName + ".bkp") |
    %{ $_ -replace 'AssemblyConfiguration\s*\(".+"\)', ('AssemblyConfiguration ("' + $Configuration.ToUpper() + '")') } |
    %{ $_ -replace 'AssemblyVersion\s*\(".+"\)', ('AssemblyVersion ("' + $SemVer.Major + '.0.0.0")') } |
    %{ $_ -replace 'AssemblyFileVersion\s*\(".+"\)', ('AssemblyFileVersion ("' + $SemVer.Major + '.' + $SemVer.Minor + '.' + $SemVer.Patch + '.0")') } |
    %{ $_ -replace 'AssemblyInformationalVersion\s*\(".+"\)', ('AssemblyInformationalVersion ("' + $SemVer + ' [' + $GitInfo.Branch + '] (Sha: ' + $GitInfo.Commit + ')")') } |
    sc $_.FullName
}

try {
  Write-Host "Restoring packages and building solutions"
  New-Item -ItemType Directory -Force -Path $OutputDir | out-null
  $SolutionFiles | %{ $_.FullName } | %{
    & $NuGet @("restore", $_, "-NonInteractive")
    & $MsBuild @($_, "/t:$Targets", "/p:Configuration=$Configuration;Platform=Any CPU;BuildRunner=$BuildRunner", "/m", "/nr:false", `
                 "/fl", "/flp:LogFile=$OutputDir\msbuild.log;Verbosity=diag")
  }
}
finally {
  Write-Host "Restoring assembly info files"
  $AssemblyInfoFiles | %{ $_.FullName } | %{ mv ($_ + ".bkp") $_ -force }
}

Write-Host "Packing nuget packages"
$NuspecFiles | %{
  & $NuGet @("pack", $_.FullName, "-Version", $SemVer, "-Properties", "Id=$($_.BaseName);Configuration=$Configuration", "-BasePath", $SourceDir, "-OutputDirectory", $OutputDir, "-Symbols", "-NoPackageAnalysis")
}