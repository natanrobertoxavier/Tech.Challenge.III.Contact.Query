using System.Text.Json.Serialization;

namespace Contact.Query.Domain.ResultServices;
public class RegionResult
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("ddd")]
    public int DDD { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }
}
