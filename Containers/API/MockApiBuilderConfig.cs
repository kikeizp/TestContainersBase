using System.Text.Json.Serialization;

namespace IntegrationTestingBase.Containers.API
{
    public record Request(
        [property: JsonPropertyName("method")] string Method,
        [property: JsonPropertyName("url")] string? Url = null,
        [property: JsonPropertyName("urlPattern")] string? UrlPattern = null,
        [property: JsonPropertyName("bodyPatterns")] List<BodyPattern>? BodyPatterns = null
    );

    public record BodyPattern(
        [property: JsonPropertyName("equalToJson")] string EqualToJson
    );

    public record Response(
        [property: JsonPropertyName("status")] int Status,
        [property: JsonPropertyName("body")] string Body,
        [property: JsonPropertyName("headers")] Dictionary<string, string> Headers
    );

    public record Mapping(
        [property: JsonPropertyName("request")] Request Request,
        [property: JsonPropertyName("response")] Response Response
    );

    public record MockApiConfig(
        [property: JsonPropertyName("mappings")] List<Mapping> Mappings
    ) : BaseConfig;

}

