namespace Palantir.Domain.Configurations
{
    using System.Text.Json.Serialization;

    public class RestApiHeader
    {
        [JsonPropertyName("AuthorizationToken")]
        public string AuthorizationToken { get; set; }

        [JsonPropertyName("UserAgent")]
        public string UserAgent { get; set; } = string.Empty;

        [JsonPropertyName("ApiVersion")]
        public string ApiVersion { get; set; } = string.Empty;
    }
}