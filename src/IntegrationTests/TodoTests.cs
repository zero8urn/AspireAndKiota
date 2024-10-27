using AspireApi.Sdk.V1.Models;
using System;
using System.Threading.Tasks;

namespace IntegrationTests;

[Collection(nameof(IntegrationFixture))]
public class TodoTests
{
    private readonly IntegrationFixture integrationFixture;

    public TodoTests(IntegrationFixture integrationFixture)
    {
        this.integrationFixture = integrationFixture;
    }

    [Fact]
    public async Task AddTodo()
    {
        var client1 = integrationFixture.ClientV1;
        TodoItem req = new()
        {
            Id = Guid.NewGuid(),
            Name = "Test" + TimeProvider.System.GetUtcNow()
        };

        // act
        var resp = await client1.Api.V1.Todos.PostAsync(req);

        // assert
        Assert.Equal(req.Id, resp.Id);
        Assert.Equal(req.Name, resp.Name);
    }
}
