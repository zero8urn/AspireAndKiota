﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Text.Json;

namespace AspireApi.Swagger;

public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;
        operation.Deprecated |= apiDescription.IsDeprecated();

        if (operation.Parameters == null)
            return;

        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
            parameter.Description ??= description.ModelMetadata.Description;

            if (parameter.Schema.Default == null && description.DefaultValue != null)
            {
                var json = JsonSerializer.Serialize(description.DefaultValue, description.ModelMetadata.ModelType);
                parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
            }

            parameter.Required |= description.IsRequired;
        }
    }
}
