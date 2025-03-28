using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace OpenApi.Extensions.Transformers;

public class TitleDocumentTransform : IOpenApiDocumentTransformer
{
    private readonly string _title;

    public TitleDocumentTransform(string title)
    {
        _title = title;
    }

    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Info.Title = _title;
        await Task.CompletedTask;
    }
}
