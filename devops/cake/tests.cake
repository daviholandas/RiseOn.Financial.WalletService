///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var configuration = Argument("configuration", "Release");
var solutionPath = "./RiseOn.Financial.WalletService.sln";

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
   Information("Deleting the folders...");
   
   var deleteDirectorySettings = new DeleteDirectorySettings 
   {
        Force = true,
        Recursive = true
   };
   
   EnsureDirectoryDoesNotExist("./tests/TestResults", deleteDirectorySettings);
   EnsureDirectoryDoesNotExist("./CoverageReport", deleteDirectorySettings);
});
    
Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Restore")
.Does(() => {
    Information("Restoring the solution...");
    DotNetRestore(solutionPath, new DotNetRestoreSettings
    {
        Verbosity = DotNetVerbosity.Minimal,
    });
});

Task("Build")
.IsDependentOn("Restore")
.Does(() => {
   Information("Building solution...");
   DotNetBuild(solutionPath, new DotNetBuildSettings
   {
        Configuration = configuration,
        Verbosity = DotNetCoreVerbosity.Minimal,
        NoIncremental = true,
        NoRestore = true
   });
});

Task("RunTests")
.IsDependentOn("Build")
.Does(() => {
    Information("Testing...");
    DotNetTest(solutionPath, new DotNetTestSettings
    {
        NoRestore = true,
        NoBuild = true,
        Configuration = configuration,
        Blame = true,
        Collectors = new string[] {"XPlat Code Coverage", "Code Coverage"},
        ResultsDirectory = new DirectoryPath("./tests/TestResults"),
        Verbosity = DotNetVerbosity.Minimal 
    });
});

Task("Report")
.IsDependentOn("RunTests")
.Does(() => {
   Information("Generating coverage report...");
   
   var arguments = new ProcessArgumentBuilder()
        .Append("\"-reports:./tests/TestResults/*/*.xml\"")
        .Append("\"-targetdir:./CoverageReport\"")
        .Append("\"-title:IntegrationTest\"");
    
   DotNetTool(solutionPath, "dotnet reportgenerator", arguments);
});
