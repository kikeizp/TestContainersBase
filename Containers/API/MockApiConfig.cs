using System.Text.Json.Serialization;

namespace IntegrationTestingBase.Containers.API
{
    public record Request
    {

        [JsonPropertyName("method")]
        public required string Method { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; } = null;
        [JsonPropertyName("urlPattern")]
        public string? UrlPattern { get; set; } = null;
        [JsonPropertyName("bodyPatterns")]
        public List<BodyPattern>? BodyPatterns { get; set; } = null;

    }

    public record BodyPattern
    {

        [JsonPropertyName("equalToJson")]
        public required string EqualToJson { get; set; }

    }

    public record Response
    {

        [JsonPropertyName("status")]
        public required int Status { get; set; }

        [JsonPropertyName("body")]
        public required string Body { get; set; }

        [JsonPropertyName("headers")]
        public required Dictionary<string, string> Headers { get; set; }

    }

    public record Mapping
    {
        [JsonPropertyName("request")]
        public required Request Request { get; set; }
        [JsonPropertyName("response")]
        public required Response Response { get; set; }
    }

    public record MockApiConfig : BaseConfig
    {

        [JsonPropertyName("mappings")]
        public required List<Mapping> Mappings { get; set; }

    }

}

