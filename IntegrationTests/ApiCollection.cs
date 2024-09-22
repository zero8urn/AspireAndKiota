using AspireApi;

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
        this.Factory = new HostFactory<AspireApi.Program>();
    }

    public void Dispose()
    {
    }
}
