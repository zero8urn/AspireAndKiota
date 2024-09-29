using Aspire.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace IntegrationTests;

[CollectionDefinition(nameof(IntegrationFixture))]
public class IntegrationCollection : ICollectionFixture<IntegrationFixture>
{
}

public class IntegrationFixture : WebApplicationFactory<AspireApi.Program>, IAsyncLifetime, IDisposable
{
    public IHost Host { get; private set; } = null!;

    public IntegrationFixture()
    {
        var options = new DistributedApplicationOptions { AssemblyName = typeof(IntegrationFixture).Assembly.FullName, DisableDashboard = true };

        var builder = DistributedApplication.CreateBuilder(options);

        var apiService = builder.AddProject("api", @"../AspireApi/AspireApi.csproj");

        builder.Services.AddKiotaHandlers();

        builder.Services.AddHttpClient<AspireApiClientFactory>((sp, client) =>
        {
            var baseUrl = apiService.GetEndpoint("http").Url;
            client.BaseAddress = new Uri(baseUrl);
        }).AttachKiotaHandlers(); // could have unwanted behaviors like a retry.

        builder.Services.AddTransient(sp => sp.GetRequiredService<AspireApiClientFactory>().GetClientV1());
        builder.Services.AddTransient(sp => sp.GetRequiredService<AspireApiClientFactory>().GetClientV2());

        Host = builder.Build();
    }

    public async Task InitializeAsync()
    {
        await Host.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        // copied from the internet, not sure if all this is needed
        await base.DisposeAsync();
        await Host.StopAsync();
        if (Host is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            Host.Dispose();
        }
    }


    public new void Dispose()
    {
    }
}