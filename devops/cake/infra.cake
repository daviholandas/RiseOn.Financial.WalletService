using SystemTask = System.Threading.Tasks.Task;

#addin nuget:?package=Cake.Docker&version=1.1.2
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var projectName = Argument("projectName","");
var dockerComposeMongo = "./devops/mongo-infra/docker-compose-mongors.yml";
var dockerCompose = "./devops/docker-compose.yml";
var composeSettings = new DockerComposeUpSettings
        {
            Files = new string[] { dockerComposeMongo, dockerCompose },
            DetachedMode = true,
            ProjectName = projectName
        };


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

Task("MongoDb")
.Does(async () => {
    Information("Creating containers of mongo in replica set.");
    DockerComposeUp(composeSettings, new string[] { "mongo1", "mongo2" });
   
    await SystemTask.Delay(TimeSpan.FromSeconds(10));

    Information("Running the command to configure replicas");
    DockerExec(container:"mongo1", command:"bash /scripts/rs-init.sh");
    Information("The mongo database was created.");
});

Task("SQLServer")
.Does(() => {
    Information("Creating SQLServer container...");
    DockerComposeUp(composeSettings, new string[] { "database" });
    Information("The SQLServer was created.");
});

Task("SonarQube")
.Does(() => {
    Information("Creating SonarQube container...");
    composeSettings.ProjectName = "RiseOn.Financial.WalletService";
    DockerComposeUp(composeSettings, new string[] { "sonarqube" });
    Information("The SonarQube was created.");
});