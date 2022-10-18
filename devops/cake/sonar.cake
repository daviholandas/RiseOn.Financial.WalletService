#l "./tests.cake"

#addin nuget:?package=Cake.DotNetTool.Module&version=2.2.0
#addin "nuget:?package=Cake.Sonar"
#tool "nuget:?package=MSBuild.SonarQube.Runner.Tool"

///////////////////////////////////////////////////////////////////////////////
// TOOLS
///////////////////////////////////////////////////////////////////////////////
#tool dotnet:?package=dotnet-sonarscanner&version=5.8.0


///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var projectKey = Argument("projectkey", "");
var projectName = Argument("projectName","");
var sonarHost = Argument("sonarhost", "http://localhost:9000");
var sonarToken = "";

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");

});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Begin")
   .Does(() => {
      SonarBegin(new SonarBeginSettings{
         Name = projectName,
         Key = projectKey,
         Url = sonarHost,
         Login = sonarToken 
      });
  });

Task("Analyze")
.IsDependentOn("Begin")
.IsDependentOn("RunTests")
.Does(() => {
    SonarEnd(new SonarEndSettings{
        Login = sonarToken
    });
});
