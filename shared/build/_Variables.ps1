$SolutionDir      = Resolve-Path "$PSScriptRoot\..\.."
$NuSpecDir        = Join-Path $SolutionDir $NuSpecDir
$OutputDir        = Join-Path $SolutionDir $OutputDir
$SourceDir        = Join-Path $SolutionDir $SourceDir
$PackagesDir      = Join-Path $SolutionDir "packages"
$SolutionFile     = Join-Path $SolutionDir $SolutionFile
$TestAssemblies   = $TestAssemblies | %{ Join-Path $SolutionDir $_ }
$CoverageConfig   = Resolve-Path "$PSScriptRoot\..\coverage.xml"

$GitInfo          = Get-GitSemVer
$SemVer           = $GitInfo.SemVer

if (IsNullOrEmpty $MsBuildDir ) { $MsBuildDir  = $LocalMsBuildDir  }
if (IsNullOrEmpty $NuGetDir   ) { $NuGetDir    = $LocalNuGetDir    }
if (IsNullOrEmpty $DotCoverDir) { $DotCoverDir = $LocalDotCoverDir }

$MsBuild  = Join-Path (Resolve-Path $MsBuildDir ) "MSBuild.exe"
$NuGet    = Join-Path (Resolve-Path $NuGetDir   ) "NuGet.exe"
$DotCover = Join-Path (Resolve-Path $DotCoverDir) "dotCover.exe"

Get-Variable -Scope 0