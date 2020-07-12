namespace Palantir.Domain.Configurations
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class WidgetConfiguration
    {
        [JsonPropertyName("WidgetType")]
        public string WidgetType { get; set; }

        [JsonPropertyName("ServiceName")]
        public string ServiceName { get; set; }

        [JsonPropertyName("RestApiUrls")]
        public List<string> RestApiUrls { get; set; }

        [JsonPropertyName("RestApiHeader")]
        public RestApiHeader RestApiHeader { get; set; }
    }
}