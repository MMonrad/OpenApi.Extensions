using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using NodaTime;

namespace OpenApi.NodaTime.Transformers.Schemas;

public class IntervalSchemaTransformer : IOpenApiSchemaTransformer
{
    private readonly Instant _instant;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public IntervalSchemaTransformer(Instant instant, JsonSerializerOptions jsonSerializerOptions)
    {
        _instant = instant;
        _jsonSerializerOptions = jsonSerializerOptions ?? throw new ArgumentNullException(nameof(jsonSerializerOptions));
    }

    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.JsonTypeInfo.Type != typeof(Interval))
        {
            return Task.CompletedTask;
        }

        var interval = new Interval(_instant,
                                    _instant.PlusTicks(TimeSpan.TicksPerDay)
                                            .PlusTicks(TimeSpan.TicksPerHour)
                                            .PlusTicks(TimeSpan.TicksPerMinute)
                                            .PlusTicks(TimeSpan.TicksPerSecond)
                                            .PlusTicks(TimeSpan.TicksPerMillisecond));

        schema.Type = JsonSchemaType.Object;
        schema.Description = "Represents a time interval between two Instant values, expressed with start and end.";
        schema.Properties = new Dictionary<string, IOpenApiSchema>
                            {
                                {
                                    nameof(Interval.Start),
                                    new OpenApiSchema
                                    {
                                        Type = JsonSchemaType.String,
                                        Format = "date-time",
                                        Example = FormatJson(interval.Start)
                                    }
                                },
                                {
                                    nameof(Interval.End),
                                    new OpenApiSchema
                                    {
                                        Type = JsonSchemaType.String,
                                        Format = "date-time",
                                        Example = FormatJson(interval.End)
                                    }
                                }
                            };

        return Task.CompletedTask;
    }

    private string FormatJson(Instant obj)
    {
        var formatToJson = JsonSerializer.Serialize(obj, _jsonSerializerOptions);
        if (formatToJson.StartsWith('"') && formatToJson.EndsWith('"'))
        {
            formatToJson = formatToJson.Substring(1, formatToJson.Length - 2);
        }

        return formatToJson;
    }
}
