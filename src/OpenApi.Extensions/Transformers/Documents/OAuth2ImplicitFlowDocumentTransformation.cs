using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace OpenApi.Extensions.Transformers.Documents;

public class OAuth2ImplicitFlowDocumentTransformation : IOpenApiDocumentTransformer
{
    private readonly string _authorizationEndpoint;
    private readonly string _description;
    private readonly string[] _scopes;
    private readonly string _securityScheme;
    private readonly string _tokenEndpoint;

    public OAuth2ImplicitFlowDocumentTransformation(string securityScheme,
        string tokenEndpoint,
        string authorizationEndpoint,
        string description,
        params string[] scopes)
    {
        _securityScheme = securityScheme;
        _tokenEndpoint = tokenEndpoint;
        _authorizationEndpoint = authorizationEndpoint;
        _description = description;
        _scopes = scopes;
    }

    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes[_securityScheme] =
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Description = _description,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(_authorizationEndpoint),
                        TokenUrl = new Uri(_tokenEndpoint),
                        Scopes = _scopes.ToDictionary(
                            s => s,
                            s => s)
                    }
                }
            };

        document.SecurityRequirements.Add(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType
                                .SecurityScheme,
                            Id = _securityScheme,
                        }
                    },
                    _scopes
                }
            });

        return Task.CompletedTask;
    }
}
