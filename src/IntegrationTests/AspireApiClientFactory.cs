using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Net.Http;

namespace IntegrationTests;

public class AspireApiClientFactory
{
    private readonly IAuthenticationProvider _authenticationProvider;
    private readonly HttpClient _httpClient;

    public AspireApiClientFactory(HttpClient httpClient)
    {
        _authenticationProvider = new AnonymousAuthenticationProvider();
        _httpClient = httpClient;
    }

    public AspireApi.Sdk.V1.AspireApiClient GetClientV1()
    {
        return new AspireApi.Sdk.V1.AspireApiClient(new HttpClientRequestAdapter(_authenticationProvider, httpClient: _httpClient));
    }

    public AspireApi.Sdk.V2.AspireApiClient GetClientV2()
    {
        return new AspireApi.Sdk.V2.AspireApiClient(new HttpClientRequestAdapter(_authenticationProvider, httpClient: _httpClient));
    }

}
