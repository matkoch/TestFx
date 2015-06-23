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

[array] `
$NuSpecFiles      = @("TestFx.Core.nuspec",
                      "TestFx.FakeItEasy.nuspec",
					  "TestFx.Farada.nuspec",
					  "TestFx.ReSharper.9.1.nuspec") | %{ Join-Path $NuSpecDir $_ }