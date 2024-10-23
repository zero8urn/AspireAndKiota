using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests;

public class HostFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseTestServer();

        //var testProjectDir = Directory.GetCurrentDirectory();
        //var solutionDir = Directory.GetParent(testProjectDir)?.Parent?.FullName;
        //var apiProjectDir = Path.Combine(solutionDir ?? "", "AspireApi");

        builder.ConfigureAppConfiguration((context, builder) =>
        {
            //// config from api project
            //builder.SetBasePath(apiProjectDir)
            builder
            .AddJsonFile("AspireApi/appsettings.json", optional: true)
            .AddJsonFile($"AspireApi/appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);

            //// override with test settings
            builder
            .AddJsonFile("AspireApi.Local/appsettings.json", optional: true)
            .AddJsonFile($"AspireApi.Local/appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();

            builder.Build();
        });

        //base.ConfigureWebHost(builder);
    }
}
