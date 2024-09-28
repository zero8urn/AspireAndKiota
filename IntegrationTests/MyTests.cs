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
    public async Task MyTest()
    {
        var client = apiFixture.Factory.CreateClient();
        var resp = await client.GetAsync("WeatherForecast");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }
}
