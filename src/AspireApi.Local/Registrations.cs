using Aspire.Hosting;
using Microsoft.Extensions.Configuration;

namespace AspireApi.Local;

public static class Registrations
{
    public const string Api = nameof(Api);
    public const string DB = nameof(DB);
    public const string TODO = "todo";

    public static IDistributedApplicationBuilder BuildHost(bool disableDashboard = false)
    {
        var options = new DistributedApplicationOptions { AssemblyName = typeof(Program).Assembly.FullName, DisableDashboard = disableDashboard };

        var builder = DistributedApplication.CreateBuilder(options); //DistributedApplicationTestingBuilder.CreateAsync() this might be another way to setup a test project

        var projectName = "AspireApi.Local";
        builder.Configuration.AddJsonFile($"{projectName}/appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"{projectName}/appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddEnvironmentVariables();

        var apiService = builder.AddProject(Api, @"../AspireApi/AspireApi.csproj");
        var https = apiService.GetEndpoint("https");

        if (!https.Exists)
        {
            apiService.WithHttpsEndpoint();
        }

        var liquibaseDirectory = builder.Configuration["Liquibase:Directory"];

        var dbPassword = builder.AddParameter("dbpassword", secret: true);
        var dbUsername = builder.AddParameter("dbusername", secret: true);
        var dbPort = int.Parse(builder.Configuration["dbport"]);

        var dbServer = builder.AddPostgres(DB, port: dbPort, userName: dbUsername, password: dbPassword)
            .WithEnvironment("POSTGRES_DB", TODO)
            .WithBindMount(
                liquibaseDirectory,
                "/docker-entrypoint-initdb.d");

        // when added as a reference to another service it injects a connection string with the database name on the end
        var db = dbServer.AddDatabase(TODO);

        builder.AddContainer("liquibase", "liquibase/liquibase", "latest")
            .WithBindMount(liquibaseDirectory, "/liquibase/changelog")
            .WithEnvironment("LIQUIBASE_COMMAND_CHANGELOG_FILE", "changelog/changelogs-root.xml")
            .WithEnvironment("LIQUIBASE_COMMAND_URL", $"jdbc:postgresql://host.docker.internal:{dbPort.ToString()}/{TODO}") // future aspire update could change to use a named docker network
            .WithEnvironment("LIQUIBASE_COMMAND_USERNAME", dbUsername.Resource.Value)
            .WithEnvironment("LIQUIBASE_COMMAND_PASSWORD", dbPassword.Resource.Value)
            .WithEnvironment("LIQUIBASE_COMMAND_DEFAULT_SCHEMA_NAME", TODO)
            .WithReference(db)
            .WithArgs("/bin/sh", "-c", "printenv && sleep 10 && liquibase update"); // the connection string for the db server is injected and instead of specifying LIQUIBASE_COMMAND_URL above it could be set in the container via a bash script. 

        apiService.WithReference(db);

        // why does this fail here, but works over in integration tests
        //var baseUrl = apiService.GetEndpoint("http");

        return builder;
    }
}
