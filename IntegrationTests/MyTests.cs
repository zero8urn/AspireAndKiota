using System.Threading.Tasks;

namespace IntegrationTests;


[Collection(nameof(ApiFixture))]
public class MyTests
{
    private readonly ApiFixture apiFixture;

    public MyTests(ApiFixture apiFixture)
    {
        this.apiFixture = apiFixture;
    }

    [Fact]
    public async Task MyTestV1()
    {
        var client = apiFixture.Factory.CreateClient();
        var resp = await client.GetAsync("api/v1/weatherforecast");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task MyTestV2()
    {
        var client = apiFixture.Factory.CreateClient();
        var resp = await client.GetAsync("api/v2/weatherforecast");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }
}
