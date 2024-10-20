using Microsoft.Extensions.Hosting;

namespace AspireApi.Extensions;

public static class EnvironmentExtensions
{
    public static string Local => nameof(Local);

    public static bool IsDevelopmentOrLocal(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsDevelopment() || hostEnvironment.IsEnvironment(Local);
    }
}
