using AspireApi.Local;

var builder = Registrations.BuildHost();

var build = builder.Build();

await build.RunAsync();

namespace AspireApi.Local
{
    public partial class Program { }
}