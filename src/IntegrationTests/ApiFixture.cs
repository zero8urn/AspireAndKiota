using AspireApi;
using System;

namespace IntegrationTests;

[CollectionDefinition(nameof(ApiFixture))]
public class ApiCollection : ICollectionFixture<ApiFixture>
{

}

public class ApiFixture : IDisposable
{
    public readonly HostFactory<Program> Factory;

    public ApiFixture()
    {
        Factory = new HostFactory<AspireApi.Program>();
    }

    public void Dispose()
    {
    }
}
