using Microsoft.AspNetCore.Authentication;

namespace AspireApi.Authentication;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public string ApiKey { get; set; } = "";
}