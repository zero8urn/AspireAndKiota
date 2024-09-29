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