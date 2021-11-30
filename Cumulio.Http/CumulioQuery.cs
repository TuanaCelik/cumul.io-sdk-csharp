using Newtonsoft.Json;
using System.Dynamic;

namespace Cumulio.Http;

public class CumulioQuery
{
    [JsonProperty("key")]
    public string Key { get; set; } = string.Empty;

    [JsonProperty("token")]
    public string Token { get; set; } = string.Empty;

    [JsonProperty("version")]
    public string Version { get; set; } = string.Empty;

    [JsonProperty("action")]
    public string Action { get; set; } = string.Empty;

    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Create, update or associate fields
    /// </summary>
    [JsonProperty("properties")]
    public ExpandoObject? Properties { get; set; } 

    /// <summary>
    /// Initial associations
    /// </summary>
    [JsonProperty("associations")]
    public List<ExpandoObject>? Associations { get; set; }

    /// <summary>
    /// Single association role
    /// </summary>
    [JsonProperty("resource")]
    public ExpandoObject? Resource { get; set; }

    /// <summary>
    /// Query filters
    /// </summary>
    [JsonProperty("find")]
    public ExpandoObject? Find { get; set; }
}
