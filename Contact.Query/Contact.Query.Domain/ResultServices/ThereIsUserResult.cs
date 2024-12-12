using Newtonsoft.Json;

namespace Contact.Query.Domain.ResultServices;
public class ThereIsUserResult
{
    [JsonProperty("thereIsUser")]
    public bool ThereIsUser { get; set; }
}
