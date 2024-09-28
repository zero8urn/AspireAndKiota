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
    public async Task MyAspireTest()
    {
        var client = integrationFixture.Host.Services.GetRequiredService<AspireApi.Sdk.V1.AspireApiClient>();
        var resp = await client.WeatherForecast.GetAsync();
        Assert.True(resp?.Count > 1);
    }
}
