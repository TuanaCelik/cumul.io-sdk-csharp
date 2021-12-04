using Newtonsoft.Json;
using System.Dynamic;
using System.Text;

namespace Cumulio.Http;

public class CumulioHttpClient
{
    private readonly HttpClient httpClient;
    private readonly HostRegion hostRegion;
    private readonly string apiVersion = "0.1.0";
    private readonly string port = "443";

    private readonly string apiKey = string.Empty;
    private readonly string apiToken = string.Empty;

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

    #region Core
    /// <summary>
    /// Sends the call to the API
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

    public async Task<dynamic?> CreateAsync(string resource, ExpandoObject properties, List<ExpandoObject>? associations = null)
    {
        if (string.IsNullOrEmpty(resource))
        {
            throw new ArgumentException($"'{nameof(resource)}' cannot be null or empty.", nameof(resource));
        }

        if (properties is null)
        {
            throw new ArgumentNullException(nameof(properties));
        }

        var query = new CumulioQuery
        {
            Action = "create",
            Properties = properties,
            Associations = associations ?? new()
        };

        return await SendAsync(resource, HttpMethod.Post, query);
    }

    public dynamic? Get(string resource, ExpandoObject filter)
    {
        try
        {
            return GetAsync(resource, filter).Result;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<dynamic?> GetAsync(string resource, ExpandoObject filter)
    {
        if (string.IsNullOrEmpty(resource))
        {
            throw new ArgumentException($"'{nameof(resource)}' cannot be null or empty.", nameof(resource));
        }

        if (filter is null)
        {
            throw new ArgumentNullException(nameof(filter));
        }

        var query = new CumulioQuery
        {
            Action = "get",
            Find = filter
        };

        return await SendAsync(resource, HttpMethod.Post, query);
    }

    public dynamic? Delete(string resource, string id)
    {
        try
        {
            return DeleteAsync(resource, id).Result;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<dynamic?> DeleteAsync(string resource, string id)
    {
        if (string.IsNullOrEmpty(resource))
        {
            throw new ArgumentException($"'{nameof(resource)}' cannot be null or empty.", nameof(resource));
        }

        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));
        }

        var query = new CumulioQuery
        {
            Action = "delete",
            Id = id
        };

        return await SendAsync(resource, HttpMethod.Delete, query);
    }

    public dynamic? Update(string resource, string id, ExpandoObject properties)
    {
        try
        {
            return UpdateAsync(resource, id, properties).Result;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<dynamic?> UpdateAsync(string resource, string id, ExpandoObject properties)
    {
        if (string.IsNullOrEmpty(resource))
        {
            throw new ArgumentException($"'{nameof(resource)}' cannot be null or empty.", nameof(resource));
        }

        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));
        }

        if (properties is null)
        {
            throw new ArgumentNullException(nameof(properties));
        }

        var query = new CumulioQuery
        {
            Action = "update",
            Id = id,
            Properties = properties
        };

        return await SendAsync(resource, HttpMethod.Patch, query);
    }

    public dynamic? Associate(string resource, string id, string associationRole, string associationId, ExpandoObject properties)
    {
        try
        {
            return AssociateAsync(resource, id, associationRole, associationId, properties).Result;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<dynamic?> AssociateAsync(string resource, string id, string associationRole, string associationId, ExpandoObject properties)
    {
        if (string.IsNullOrEmpty(resource))
        {
            throw new ArgumentException($"'{nameof(resource)}' cannot be null or empty.", nameof(resource));
        }

        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));
        }

        if (string.IsNullOrEmpty(associationRole))
        {
            throw new ArgumentException($"'{nameof(associationRole)}' cannot be null or empty.", nameof(associationRole));
        }

        if (string.IsNullOrEmpty(associationId))
        {
            throw new ArgumentException($"'{nameof(associationId)}' cannot be null or empty.", nameof(associationId));
        }

        if (properties is null)
        {
            throw new ArgumentNullException(nameof(properties));
        }

        dynamic association = new ExpandoObject();
        association.role = associationRole;
        association.id = associationId;

        var query = new CumulioQuery
        {
            Action = "associate",
            Id = id,
            Resource = association,
            Properties = properties
        };

        return await SendAsync(resource, HttpMethod.Post, query);
    }

    public dynamic? Dissociate(string resource, string id, string associationRole, string associationId)
    {
        try
        {
            return DissociateAsync(resource, id, associationRole, associationId).Result;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<dynamic?> DissociateAsync(string resource, string id, string associationRole, string associationId)
    {
        if (string.IsNullOrEmpty(resource))
        {
            throw new ArgumentException($"'{nameof(resource)}' cannot be null or empty.", nameof(resource));
        }

        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentException($"'{nameof(id)}' cannot be null or empty.", nameof(id));
        }

        if (string.IsNullOrEmpty(associationRole))
        {
            throw new ArgumentException($"'{nameof(associationRole)}' cannot be null or empty.", nameof(associationRole));
        }

        if (string.IsNullOrEmpty(associationId))
        {
            throw new ArgumentException($"'{nameof(associationId)}' cannot be null or empty.", nameof(associationId));
        }

        dynamic association = new ExpandoObject();
        association.role = associationRole;
        association.id = associationId;

        var query = new CumulioQuery
        {
            Action = "dissociate",
            Id = id,
            Resource = association
        };

        return await SendAsync(resource, HttpMethod.Post, query);
    }
    #endregion

    #region Services
    public dynamic? QueryData(ExpandoObject filter)
    {
        try
        {
            return Get("data", filter);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<dynamic?> QueryDataAsync(ExpandoObject filter)
    {
        return await GetAsync("data", filter);
    }
    #endregion

    #region Implementations
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
    #endregion
}
