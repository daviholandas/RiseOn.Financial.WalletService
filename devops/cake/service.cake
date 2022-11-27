#addin nuget:?package=Cake.Docker
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var dockerCompose = "./devops/docker-compose.yaml";

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

Task("Publish-Docker")
.Does(() => {
   Information("Dockering the service...");
   DockerComposeUp(new DockerComposeUpSettings
                       {
                           Files = new string[] { dockerCompose },
                           DetachedMode = true,
                           ProjectName = "riseon-financial-walletservice"
                       });
});

Task("Publish-Container")
.Does(() => {
   Information("Publishing the service as container...");
   DotNetPublishSettings settings = new() 
   {
      Configuration = "Release",
      Verbosity = DotNetVerbosity.Minimal,
      ArgumentCustomization = args => args
         .Append(" --os linux")
         .Append(" --arch x64")
         .Append(" -p:PublishProfile=DefaultContainer")
   };

   DotNetPublish(solutionPath, settings);
});
