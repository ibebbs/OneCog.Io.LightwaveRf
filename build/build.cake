#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#addin "Cake.ExtendedNuGet"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("../src/OneCog.Io.LightwaveRf/bin") + Directory(configuration);
var sourceDir = "src";
var solutionPath = "OneCog.Io.LightwaveRf";
var solutionName = "OneCog.Io.LightwaveRf.sln";
var solutionPath = "../" + sourceDir + "/" + solutionName;

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean").Does(() => CleanDirectory(buildDir));

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() => NuGetRestore(solutionPath));

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(
		() =>
		{
			if(IsRunningOnWindows())
			{
			  // Use MSBuild
			  MSBuild(solutionPath, settings => settings.SetConfiguration(configuration));
			}
			else
			{
			  // Use XBuild
			  XBuild(solutionPath, settings => settings.SetConfiguration(configuration));
			}
		});
	
Task("Package")
    .IsDependentOn("Build")
    .Does(
		() =>
		{
			var profiles = new [] { "net45", "portable45-net45+win8" };
			var files = new [] { "OneCog.Io.LightwaveRf.dll", "OneCog.Io.LightwaveRf.pdb" };

			var content = profiles.SelectMany(profile => files.Select(file => new NuSpecContent { Source = $"{buildDir}/{file}" Target = $"{platform}/{file}")).ToArray();

			var nugetSettings = new NugetPackSettings
			{
                Id                      = "OneCog.Io.LightwaveRf",
                Version                 = "0.0.0.1",
                Title                   = "OneCog.Io.LightwaveRf",
                Authors                 = new[] { "onecog.solutions" },
                Owners                  = new[] { "onecog.solutions" },
                Description             = "A portable assembly for interacting with LightwaveRf WifiLink",
                Summary                 = "A portable assembly for interacting with LightwaveRf WifiLink",
                ProjectUrl              = new Uri("https://github.com/ibebbs/OneCog.Io.LightwaveRf"),
                LicenseUrl              = new Uri("https://github.com/ibebbs/OneCog.Io.LightwaveRf/blob/master/License.txt"),
                Copyright               = "onecog.solutions 2015",
                Tags                    = new [] { "LightwaveRf", "Portable" },
                RequireLicenseAcceptance= false,
                Symbols                 = false,
                NoPackageAnalysis       = true,
                Files                   = content
			};

			NuGetPack("OneCog.Io.LightwaveRf.nuspec", ​nugetSettings);
		});
		
//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default").IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);