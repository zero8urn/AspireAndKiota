using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspireApi;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = $"{description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = description.IsDeprecated ? "This version is deprecated" : "My Api",
            });
        }

        options.DescribeAllParametersInCamelCase();
        options.UseInlineDefinitionsForEnums();

        //options.TagActionsBy(x =>
        //{
        //    var controller = x.ActionDescriptor as ControllerActionDescriptor;

        //    return [controller?.ControllerName.Substring(0, controller.ControllerName.ToLower().IndexOf("controller"))];
        //});

        options.OrderActionsBy(x => x.RelativePath);

        options.AddSecurityDefinition("Key", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.ApiKey,
            Description = "Api Key",
            Name = "x-api-key",
            In = ParameterLocation.Path
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Key"
                    }
                },
                []
            }
        });
    }
}

