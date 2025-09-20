using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace OpenApi.Extensions.Schemas;

public class DescribeAllEnumsAsStringsSchemaTransformation : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.JsonTypeInfo.Type is { IsEnum: true })
        {
            schema.Enum.Clear();
            Enum.GetNames(context.JsonTypeInfo.Type)
                .ToList()
                .ForEach(name => schema.Enum.Add(new OpenApiString($"{name}")));

            schema.Type = "string";
        }

        return Task.CompletedTask;
    }
}
