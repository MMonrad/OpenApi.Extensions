using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace OpenApi.Extensions.Schemas;

public class DescribeAllEnumsAsStringsSchemaTransformation : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.JsonTypeInfo.Type is { IsEnum: true })
        {
            schema.Enum ??= new List<JsonNode>();
            schema.Enum.Clear();
            Enum.GetNames(context.JsonTypeInfo.Type)
                .ToList()
                .ForEach(name => schema.Enum.Add($"{name}"));

            schema.Type = JsonSchemaType.String;
        }

        return Task.CompletedTask;
    }
}
