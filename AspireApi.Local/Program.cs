using AspireApi.Local;

var builder = Registrations.BuildHost();

var build = builder.Build();

//var dbResource = (PostgresServerResource)builder.Resources.First(x => x.Name == Registrations.DB);

//var dbService = builder.CreateResourceBuilder(dbResource);
//var b = dbResource.GetEndpoint("tcp");

await build.RunAsync();

namespace AspireApi.Local
{
    public partial class Program { }
}