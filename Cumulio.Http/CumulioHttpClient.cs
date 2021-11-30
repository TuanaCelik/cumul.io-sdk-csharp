using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http.Json;
using System.Text;

namespace Cumulio.Http;

public class CumulioHttpClient
{
    private readonly HttpClient httpClient;
    private readonly HostRegion hostRegion;
    private readonly string apiVersion = "0.1.0";
    private readonly string port = "443";

    //Demo account
    //private readonly string apiKey = "d4f04e03-d90a-47fc-a676-d50e97be09db";
    //private readonly string apiToken = "bw9F62m3CwuKO66uNH1imYrSNqfjpo4twieKSCUxZMHuIJ4RtQFVkNTxk2Qiy2S80AS89vkstVrCYhoz4RKBjW1LN9BUcjGL7wol28Jzk0uCD1B7N9lbOIWbEBMTMKqU1d6BVjiXD6MtRVShKyEwz6";

    //Netrush account
    private string apiKey = "a54e6eea-14d7-4066-8cc4-6bb2ffbb3bb8";
    private string apiToken = "aK9HWf0J7dIEilLCob1MuBdDT9giPaQe1OI6RGj6aFklvDd4iU5Gab6CNUbyttxqQC2QF6pMal7mv9gC1HwmEtUFqnkt2cC0tFVc1Cnzn2OngOOT1Zm6HG3ShJSocPYj6Ek2zOl5nThLsQDltevveZ";

    public CumulioHttpClient(HostRegion hostRegion, string apiKey, string apiToken, string apiVersion = "0.1.0", string port = "443")
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new ArgumentException($"'{nameof(apiKey)}' cannot be null or empty.", nameof(apiKey));
        }

        if (string.IsNullOrEmpty(apiToken))
        {
            throw new ArgumentException($"'{nameof(apiToken)}' cannot be null or empty.", nameof(apiToken));
        }

        if (string.IsNullOrEmpty(apiVersion))
        {
            throw new ArgumentException($"'{nameof(apiVersion)}' cannot be null or empty.", nameof(apiVersion));
        }

        if (string.IsNullOrEmpty(port))
        {
            throw new ArgumentException($"'{nameof(port)}' cannot be null or empty.", nameof(port));
        }

        this.hostRegion = hostRegion;
        this.apiKey = apiKey;
        this.apiToken = apiToken;
        this.apiVersion = apiVersion;
        this.port = port;

        var host = "https://api" + (hostRegion == HostRegion.US ? ".us" : "") + ".cumul.io/";
        httpClient = new() { BaseAddress = new Uri(host) };
    }
    public CumulioHttpClient(CumulioSettings cumulioSettings) : this(
        hostRegion: (cumulioSettings?.Host?.ToLower() ?? string.Empty).Contains(".us.") ? HostRegion.US : HostRegion.EU,
        apiKey: cumulioSettings?.APIKey ?? string.Empty,
        apiToken: cumulioSettings?.APIToken ?? string.Empty,
        apiVersion: cumulioSettings?.APIVersion ?? "0.1.0",
        port: cumulioSettings?.Port ?? "443")
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="resource">The resource or entity type you are performing the action on. IE: authorization</param>
    /// <param name="httpMethod">The HttpMethod used to submit the request</param>
    /// <param name="query">The query object that gets serialized to the request body</param>
    /// <returns></returns>
    private async Task<dynamic?> SendAsync(string resource, HttpMethod httpMethod, CumulioQuery query)
    {
        query.Key = apiKey;
        query.Token = apiToken;
        query.Version = apiVersion;

        try
        {
            var request = new HttpRequestMessage(httpMethod, $"{apiVersion}/{resource}");
            request.Content = new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(request);

            var result = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

            return result;
        }
        catch (Exception ex)
        {
            throw new CumulioException(ex);
        }
    }

    public dynamic? Create(string resource, ExpandoObject properties, List<ExpandoObject>? associations = null)
    {
        try
        {
            return CreateAsync(resource, properties, associations).Result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<dynamic?> CreateAsync(string resource, ExpandoObject properties, List<ExpandoObject>? associations = null)
    {
        var query = new CumulioQuery
        {
            Action = "create",
            Properties = properties,
            Associations = associations ?? new()
        };

        return await SendAsync(resource, HttpMethod.Post, query);
    }

    public async Task<dynamic?> CreateAuthorizationAsync(string username, string name, string email, IEnumerable<string> securables, string expiry = "24 hours", string inactivityInterval = "10 minutes")
    {
        dynamic properties = new ExpandoObject();
        properties.type = "temporary";
        properties.expiry = expiry;
        properties.securables = securables;
        properties.username = username;
        properties.name = name;
        properties.email = email;
        properties.inactivity_interval = inactivityInterval;
        
        return await CreateAsync("authorization", properties);        
    }
}
