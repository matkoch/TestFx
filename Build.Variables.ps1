$SolutionDir      = Get-ScriptDirectory
$NuSpecDir        = Join-Path $SolutionDir $NuSpecDir
$OutputDir        = Join-Path $SolutionDir $OutputDir
$SourceDir        = Join-Path $SolutionDir $SourceDir
$PackagesDir      = Join-Path $SolutionDir "packages"
$ToolsDir         = Join-Path $SolutionDir "tools"
$SolutionFile     = Join-Path $SolutionDir $SolutionFile
$TestAssemblies   = $TestAssemblies | %{ Join-Path $SolutionDir $_ }

$GitInfo          = Get-GitSemVer
$SemVer           = $GitInfo.SemVer

$MsBuildExe       = "msbuild.exe"
$NuGetExe         = "NuGet.exe"
$NUnitExe         = "nunit-console-x86.exe"
$DotCoverExe      = "dotCover.exe"

$MsBuild          = Join-Path (Resolve-Path $MsBuildDir   ) $MsBuildExe
$NuGet            = Join-Path (Resolve-Path $NuGetDir     ) $NuGetExe

$EchoArgs = (Join-Path $SolutionDir "EchoArgs.exe")

Get-Variable -Scope 0