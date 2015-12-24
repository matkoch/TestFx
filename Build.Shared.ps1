$SolutionFile     = "TestFx.sln"
$SourceDir        = "src"
$NuSpecDir        = "nuspec"
$OutputDir        = "output"

$CoverageFile     = Join-Path $SourceDir "coverage.xml"
$AssemblyInfoFile = Join-Path $SourceDir "AssemblyInfoShared.cs"

[array] `
$TestAssemblies   = @("TestFx.SpecK.Tests",
					  "TestFx.MSpec.Tests") | %{ Join-Path $SourceDir "$_\bin\$Configuration\$_.dll" }

[array] `
$NuSpecFiles      = @("TestFx.Core.nuspec",
					  "TestFx.ReSharper.nuspec",

					  "TestFx.SpecK.nuspec",
					  "TestFx.MSpec.nuspec",

                      "TestFx.FakeItEasy.nuspec",
					  "TestFx.Farada.nuspec") | %{ Join-Path $NuSpecDir $_ }