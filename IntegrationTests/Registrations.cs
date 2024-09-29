using Aspire.Hosting;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System;
using System.Net.Http;

namespace IntegrationTests;

public static class Registrations
{
    public static IDistributedApplicationBuilder BuildHost()
    {
        var options = new DistributedApplicationOptions { AssemblyName = typeof(IntegrationFixture).Assembly.FullName, DisableDashboard = true };

        var builder = DistributedApplication.CreateBuilder(options); //DistributedApplicationTestingBuilder.CreateAsync() this might be another way to setup a test project

        var apiService = builder.AddProject("api", @"../AspireApi/AspireApi.csproj");

        builder.Services.AddKiotaHandlers();

        builder.Services.AddHttpClient<AspireApiClientFactory>((sp, client) =>
        {
            var baseUrl = apiService.GetEndpoint("http").Url;
            client.BaseAddress = new Uri(baseUrl);
        }).AttachKiotaHandlers(); // could have unwanted behaviors like a retry.

        builder.Services.AddTransient(sp => sp.GetRequiredService<AspireApiClientFactory>().GetClientV1());

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
