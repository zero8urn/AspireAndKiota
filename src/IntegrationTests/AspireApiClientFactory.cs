using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Net.Http;

namespace IntegrationTests;

public class AspireApiClientFactory
{
    private readonly HttpClient _httpClient;
    private readonly IAuthenticationProvider authProvider;

    public AspireApiClientFactory(HttpClient httpClient,
        IAuthenticationProvider authProvider)
    {
        _httpClient = httpClient;
        this.authProvider = authProvider;
    }

    public AspireApi.Sdk.V1.AspireApiClient GetClientV1()
    {
        return new AspireApi.Sdk.V1.AspireApiClient(new HttpClientRequestAdapter(authProvider, httpClient: _httpClient));
    }

    public AspireApi.Sdk.V2.AspireApiClient GetClientV2()
    {
        return new AspireApi.Sdk.V2.AspireApiClient(new HttpClientRequestAdapter(authProvider, httpClient: _httpClient));
    }

}
