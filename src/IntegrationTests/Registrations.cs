using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using AspireApi.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System;
using System.Linq;
using System.Net.Http;

namespace IntegrationTests;

public static class Registrations
{
    public static IDistributedApplicationBuilder AddConfiguration(this IDistributedApplicationBuilder builder)
    {
        var aspireApi = "AspireApi";
        var aspireApiLocal = "AspireApi.Local";
        builder.Configuration.AddJsonFile($"{aspireApi}/appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"{aspireApi}/appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"{aspireApiLocal}/appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"{aspireApiLocal}/appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddEnvironmentVariables();
        return builder;
    }

    public static IDistributedApplicationBuilder BuildHost()
    {
        var builder = AspireApi.Local.Registrations.BuildHost(true);

        builder.AddConfiguration();

        var apiServiceResource = (ProjectResource)builder.Resources.First(x => x.Name == AspireApi.Local.Registrations.Api);

        var apiService = builder.CreateResourceBuilder(apiServiceResource);

        builder.Services.AddKiotaHandlers();

        builder.Services.AddSingleton<IAuthenticationProvider, ApiKeyAuthenticationProvider>(sp =>
        {
            var apiKey = builder.Configuration.GetValue<string>("ApiKey");

            return new ApiKeyAuthenticationProvider(apiKey, ApiKeyConstants.HeaderName, ApiKeyAuthenticationProvider.KeyLocation.Header);
        });

        builder.Services.AddHttpClient<AspireApiClientFactory>((sp, client) =>
        {
            var baseUrl = apiService.GetEndpoint("https").Url;
            client.BaseAddress = new Uri(baseUrl);
        }).AttachKiotaHandlers(); // could have unwanted behaviors like a retry.

        builder.Services.AddTransient(sp => sp.GetRequiredService<AspireApiClientFactory>().GetClientV1());
        builder.Services.AddTransient(sp => sp.GetRequiredService<AspireApiClientFactory>().GetClientV2());

        return builder;
    }

    public static IServiceCollection AddKiotaHandlers(this IServiceCollection services)
    {
        // Dynamically load the Kiota handlers from the Client Factory
        var kiotaHandlers = KiotaClientFactory.GetDefaultHandlerTypes();
        // And register them in the DI container
        foreach (var handler in kiotaHandlers)
        {
            services.AddTransient(handler);
        }

        return services;
    }

    public static IHttpClientBuilder AttachKiotaHandlers(this IHttpClientBuilder builder)
    {
        // Dynamically load the Kiota handlers from the Client Factory
        var kiotaHandlers = KiotaClientFactory.GetDefaultHandlerTypes();
        // And attach them to the http client builder
        foreach (var handler in kiotaHandlers)
        {
            builder.AddHttpMessageHandler((sp) => (DelegatingHandler)sp.GetRequiredService(handler));
        }

        return builder;
    }
}
