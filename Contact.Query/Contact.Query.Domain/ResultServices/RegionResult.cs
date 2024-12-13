using Newtonsoft.Json;

namespace Contact.Query.Domain.ResultServices;
public class RegionResult
{
    [JsonProperty("ddd")]
    public int DDD { get; set; }

    [JsonProperty("region")]
    public string Region { get; set; }
}
