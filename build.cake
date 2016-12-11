//#addin "Cake.ExtendedNuGet"

#tool "nuget:?package=NUnit.Runners&version=2.6.4"
#tool "nuget:?package=JetBrains.ReSharper.CommandLineTools"
#tool "nuget:?package=JetBrains.dotCover.CommandLineTools"
#tool "nuget:?package=OpenCover"


var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");


var solutionFile = File("./TestFx.sln");
var outputDirectory = Directory("./output");
var inspectcodeCacheDirectory = Directory("./_ReSharper.InspectionCache");

var msbuildLogFile = outputDirectory + File("msbuild.log");
var testAssembliesPattern = "./src/**/bin/" + configuration + "/TestFx.*.Tests.dll";
var nunitResultsFile = outputDirectory + File("nunit.xml");
var dotcoverResultsFile = outputDirectory + File("dotcover.xml");
var opencoverResultsFile = outputDirectory + File("opencover.xml");
var inspectcodeResultsFile = outputDirectory + File("inspectcode.xml");

public static class DefaultNuGetPackSettings
{
    public static NuGetPackSettings Create(ConvertableDirectoryPath outputDirectory)
    {
        return new NuGetPackSettings {
            Version                  = "0.0.0.1",
            Authors                  = new[] {"Matthias Koch"},
            Owners                   = new[] {"matkoch"},
            Summary                  = "Excellent summary of what the package does",
            ProjectUrl               = new Uri("https://github.com/matkoch/TestFx/"),
            LicenseUrl               = new Uri("https://raw.github.com/matkoch/TestFx/master/LICENSE"),
            IconUrl                  = new Uri("http://matkoch.github.io/TestFx/TestFx.ico"),
            Copyright                = "Copyright (c) Matthias Koch, 2014-2016",
            BasePath                 = "./src",
            OutputDirectory          = outputDirectory,
            Symbols                  = false,
            NoPackageAnalysis        = true,
            RequireLicenseAcceptance = false
          };
    }
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(outputDirectory);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionFile);
});

Task("Compile")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var settings = new MSBuildSettings {
        ToolVersion = MSBuildToolVersion.VS2015,
        MaxCpuCount = 0,
        NodeReuse = false,
        Configuration = configuration,
        PlatformTarget = PlatformTarget.MSIL
      };

    if (TeamCity.IsRunningOnTeamCity)
      settings.SetVerbosity(Verbosity.Diagnostic);
    else
      settings
        .SetVerbosity(Verbosity.Quiet)
        .AddFileLogger(
          new MSBuildFileLogger {
              Verbosity = Verbosity.Diagnostic,
              LogFile = msbuildLogFile
            });

    MSBuild(solutionFile, settings);
    //Zip("./", "cakeassemblies.zip", "src/**/bin/" + configuration + "/*");
});

Task("TestCover")
    //.IsDependentOn("Compile")
    .Does(() =>
{
    OpenCover(tool =>
      tool.NUnit(testAssembliesPattern, new NUnitSettings {
          ResultsFile = nunitResultsFile,
          ShadowCopy = false
        }),
      opencoverResultsFile,
      new OpenCoverSettings()
        .WithFilter("+[TestFx*]*")
        .WithFilter("-[TestFx.TestInfrastructure]*")
        .WithFilter("-[TestFx.*.Tests]*")

        .WithFilter("-[*]JetBrains.*")
        .WithFilter("-[*]Costura.*")
        .ExcludeByAttribute("System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute"));

    TeamCity.ImportData("nunit", nunitResultsFile);
    //TeamCity.ImportDotCoverCoverage(dotcoverResultsFile);

    Information(XmlPeek(opencoverResultsFile, "/CoverageSession/Summary/@sequenceCoverage"));
});

Task("InspectCode")
    .Does(() =>
{
    InspectCode(solutionFile, new InspectCodeSettings {
      SolutionWideAnalysis = true,
      OutputFile = inspectcodeResultsFile,
      CachesHome = inspectcodeCacheDirectory
    });
});

Task("FxCop")
    .IsDependentOn("Restore")
    .Does(() =>
{
    MSBuild(solutionFile, new MSBuildSettings {
        ToolVersion = MSBuildToolVersion.VS2015,
        MaxCpuCount = 0,
        NodeReuse = false,
        Configuration = configuration,
        PlatformTarget = PlatformTarget.MSIL,
      }.WithProperty("RunCodeAnalysis", new[]{ "true" })); // PR: params + copy

    var codeAnalysisFiles = GetFiles("./**/*.CodeAnalysisLog.xml");
    CopyFiles(codeAnalysisFiles, outputDirectory);
});

Task("Packaging")
    //.IsDependentOn("Compile")
    .Does(() =>
{
    var corePackage = DefaultNuGetPackSettings.Create(outputDirectory);
    corePackage.Id = "TestFx.Core";
    corePackage.Description = "TestFx core components.";
    corePackage.Tags = new[] { "testfx", "testing", "framework" };
    corePackage.Files = new[] {
        new NuSpecContent { Target = "lib/net40", Source = "TestFx.Core\\bin\\" + configuration + "\\*" },
        new NuSpecContent { Target = "content", Source = "TestFx.Core\\NuGet\\content\\**\\*" },
        new NuSpecContent { Target = "tools", Source = "TestFx.Console\\bin\\" + configuration + "\\TestFx.exe" }
      };

    var speckPackage = DefaultNuGetPackSettings.Create(outputDirectory);
    speckPackage.Id = "TestFx.SpecK";
    speckPackage.Description = "Behavior-driven design with specifications.";
    speckPackage.Tags = new[] { "testfx", "testing", "speck", "bdd", "tdd" };
    speckPackage.Files = new[] {
        new NuSpecContent { Target = "lib/net40", Source = "TestFx.SpecK\\bin\\" + configuration + "\\TestFx.SpecK.*" },
        new NuSpecContent { Target = "content", Source = "TestFx.SpecK\\NuGet\\content\\**\\*" },
        new NuSpecContent { Target = "tools", Source = "TestFx.SpecK\\NuGet\\tools\\**\\*" }
      };
    speckPackage.Dependencies = new[] {
        new NuSpecDependency { Id = "TestFx.Core" },
      };

    var mspecPackage = DefaultNuGetPackSettings.Create(outputDirectory);
    mspecPackage.Id = "TestFx.MSpec";
    mspecPackage.Description = "Behavior-driven design with specifications.";
    mspecPackage.Tags = new[] { "testfx", "testing", "mspec", "bdd", "tdd" };
    mspecPackage.Files = new[] {
        new NuSpecContent { Target = "lib/net40", Source = "TestFx.MSpec\\bin\\" + configuration + "\\TestFx.MSpec.*" },
        new NuSpecContent { Target = "content", Source = "TestFx.MSpec\\NuGet\\content\\**\\*" },
        new NuSpecContent { Target = "tools", Source = "TestFx.MSpec\\NuGet\\tools\\**\\*" }
      };
    mspecPackage.Dependencies = new[] {
        new NuSpecDependency { Id = "TestFx.Core" },
        new NuSpecDependency { Id = "Machine.Specifications", Version = "[0.9, 1.0)" }
      };

    var fakeiteasyPackage = DefaultNuGetPackSettings.Create(outputDirectory);
    fakeiteasyPackage.Id = "TestFx.FakeItEasy";
    fakeiteasyPackage.Description = "Integration for FakeItEasy.";
    fakeiteasyPackage.Tags = new[] { "testfx", "testing", "fakeiteasy" };
    fakeiteasyPackage.Files = new[] {
        new NuSpecContent { Target = "lib/net40", Source = "TestFx.FakeItEasy\\bin\\" + configuration + "\\TestFx.FakeItEasy.*" },
        new NuSpecContent { Target = "content", Source = "TestFx.MSpec\\NuGet\\content\\**\\*" }
      };
    fakeiteasyPackage.Dependencies = new[] {
        new NuSpecDependency { Id = "TestFx.Core" },
        new NuSpecDependency { Id = "TestFx.SpecK" },
        new NuSpecDependency { Id = "FakeItEasy", Version = "[1.25, 2.0)" }
      };

    var faradaPackage = DefaultNuGetPackSettings.Create(outputDirectory);
    faradaPackage.Id = "TestFx.Farada";
    faradaPackage.Description = "Integration for Farada.";
    faradaPackage.Tags = new[] { "testfx", "testing", "farada" };
    faradaPackage.Files = new[] {
        new NuSpecContent { Target = "lib/net40", Source = "TestFx.Farada\\bin\\" + configuration + "\\TestFx.Farada.*" },
        new NuSpecContent { Target = "content", Source = "TestFx.Farada\\NuGet\\content\\**\\*" }
      };
    faradaPackage.Dependencies = new[] {
        new NuSpecDependency { Id = "TestFx.Core" },
        new NuSpecDependency { Id = "TestFx.SpecK" },
        new NuSpecDependency { Id = "Farada.TestDataGeneration", Version = "[0.1.6, 2.0)" }
      };

    var resharperPackage = DefaultNuGetPackSettings.Create(outputDirectory);
    resharperPackage.Id = "ReSharper.TestFx";
    resharperPackage.Description = "ReSharper runner for TestFx-based test frameworks.";
    resharperPackage.Tags = new[] { "testfx", "testing", "speck", "mspec", "bdd", "tdd" };
    resharperPackage.Files = new[] {
        new NuSpecContent { Target = "DotFiles", Source = "TestFx.ReSharper\\bin\\" + configuration + "\\Autofac.*" },
        new NuSpecContent { Target = "DotFiles", Source = "TestFx.ReSharper\\bin\\" + configuration + "\\TestFx.*" },
        new NuSpecContent { Target = "DotFiles\\Extensions\\ReSharper.TestFx\\settings", Source = "TestFx.ReSharper\\*.DotSettings" }
      };
    resharperPackage.Dependencies = new[] {
        new NuSpecDependency { Id = "wave", Version = "[6.0]" }
    };

    NuGetPack(corePackage);
    NuGetPack(speckPackage);
    NuGetPack(mspecPackage);
    NuGetPack(fakeiteasyPackage);
    NuGetPack(faradaPackage);
    NuGetPack(resharperPackage);
});

Task("Test")
    .Does(() => {
    var dict = Environment.GetEnvironmentVariables()
                .Cast<System.Collections.DictionaryEntry>();

    foreach (var x in dict)
      Information(x.Key + " = " + x.Value);

});

Task("CodeAnalysis")
    .IsDependentOn("InspectCode")
    .IsDependentOn("FxCop");

Task("Default")
    .IsDependentOn("Test");
    //.IsDependentOn("CodeAnalysis")


RunTarget(target);