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
        .SetVerbosity(Verbosity.Minimal)
        .AddFileLogger(
          new MSBuildFileLogger {
              Verbosity = Verbosity.Diagnostic,
              LogFile = msbuildLogFile
            });

    MSBuild(solutionFile, settings);
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

Task("Package")
    .IsDependentOn("Compile")
    .Does(() =>
{
   // var nuGetPackSettings   = new NuGetPackSettings {
   //     Version                  = "0.0.0.1",
   //     Authors                  = new[] {"Matthias Koch"},
   //     Owners                   = new[] {"matkoch"},
   //     Summary                  = "Excellent summary of what the package does",
   //     ProjectUrl               = new Uri("https://github.com/matkoch/TestFx/"),
   //     LicenseUrl               = new Uri("https://raw.github.com/matkoch/TestFx/master/LICENSE"),
   //     IconUrl                  = new Uri("http://matkoch.github.io/TestFx/TestFx.ico"),
   //     Copyright                = "Copyright © Matthias Koch, 2014-2016",
   //     BasePath                 = "./src",
   //     OutputDirectory          = outputDirectory,
   //     Symbols                  = false,
   //     NoPackageAnalysis        = true,
   //     RequireLicenseAcceptance = false
   //   };

        //Id                       = "TestNuget",
        //Description              = "The description of the package",
        //Title                    = "The tile of the package",
        //ReleaseNotes             = new [] {"Bug fixes", "Issue fixes", "Typos"},
        //Tags                     = new [] {"Cake", "Script", "Build"},


        //Files                    = new [] {
        //                                      new NuSpecContent {Source = "bin/TestNuget.dll", Target = "bin"},
        //                                  },

});

Task("CodeAnalysis")
    .IsDependentOn("InspectCode")
    .IsDependentOn("FxCop");

Task("Default")
    .IsDependentOn("TestCover");
    //.IsDependentOn("CodeAnalysis");

RunTarget(target);