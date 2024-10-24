using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspireApi.Authentication;

public static class Registrations
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        var apiKeyConfig = config.GetSection(nameof(ApiKeyAuthenticationOptions));
        var apiKeyOptions = apiKeyConfig.Get<ApiKeyAuthenticationOptions>();

        services.Configure<ApiKeyAuthenticationOptions>(apiKeyConfig);

        services.AddAuthentication()
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                ApiKeyConstants.SchemeName,
                options =>
                {
                    options.ApiKey = apiKeyOptions.ApiKey;
                });

        services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(ApiKeyConstants.SchemeName)
            .RequireAuthenticatedUser()
            .Build());

        return services;
    }
}
