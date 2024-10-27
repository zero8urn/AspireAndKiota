using System.Threading.Tasks;

namespace IntegrationTests;

[Collection(nameof(IntegrationFixture))]
public class MyAspireTests
{
    private readonly IntegrationFixture integrationFixture;

    public MyAspireTests(IntegrationFixture integrationFixture)
    {
        this.integrationFixture = integrationFixture;
    }

    [Fact]
    public async Task MyAspireTestV1()
    {
        var client = integrationFixture.ClientV1;
        var resp = await client.Api.V1.Weatherforecast.GetAsync();
        Assert.True(resp?.Count > 1);
    }

    [Fact]
    public async Task MyAspireTestV2()
    {
        var client = integrationFixture.ClientV2;
        var resp = await client.Api.V2.Weatherforecast.GetAsync();
        Assert.True(resp?.Count > 1);
    }
}
