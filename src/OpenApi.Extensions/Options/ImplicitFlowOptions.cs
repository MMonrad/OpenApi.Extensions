namespace OpenApi.Extensions.Options;

public class ImplicitFlowOptions
{
    public string AuthorizationEndpoint { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string[] Scopes { get; set; } = [];
    public string SecurityScheme { get; set; } = null!;
    public string TokenEndpoint { get; set; } = null!;
}
