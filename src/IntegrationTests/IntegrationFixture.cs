using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace IntegrationTests;

[CollectionDefinition(nameof(IntegrationFixture))]
public class IntegrationCollection : ICollectionFixture<IntegrationFixture>
{
}

public class IntegrationFixture : IAsyncLifetime, IDisposable
{
    public IHost Host { get; private set; } = null!;

    public AspireApi.Sdk.V1.AspireApiClient ClientV1 => Host.Services.GetRequiredService<AspireApi.Sdk.V1.AspireApiClient>();
    public AspireApi.Sdk.V2.AspireApiClient ClientV2 => Host.Services.GetRequiredService<AspireApi.Sdk.V2.AspireApiClient>();

    public IntegrationFixture()
    {
        var builder = Registrations.BuildHost();

        Host = builder.Build();
    }

    public async Task InitializeAsync()
    {
        await Host.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Host.StopAsync();
        Host.Dispose();
    }


    public void Dispose()
    {
    }
}