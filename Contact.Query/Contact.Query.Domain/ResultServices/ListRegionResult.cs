using System.Text.Json.Serialization;

namespace Contact.Query.Domain.ResultServices;
public class ListRegionResult
{
    [JsonPropertyName("regionsDDD")]
    public IEnumerable<RegionResult> RegionsDDD { get; set; }
}
