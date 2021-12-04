using Newtonsoft.Json;

namespace Cumulio.Http.Models;

public class Account
{
    /// <summary>
    /// Unique key of the account (automatically assigned)
    /// </summary>
    [JsonProperty("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Type of data provider to retrieve data from. Either the 'slug' of a plugin, 
    /// e.g. googleanalytics, googledrive, quandl, salesforce, mailchimp, etc... 
    /// the slug of your own plugin or one of a database such as 'postgresql'. 
    /// Slugs are unique short names for plugins and dataproviders (such as dbs)
    /// </summary>
    [JsonProperty("provider")]
    public string? Provider { get; set; }

    /// <summary>
    /// Date/time this account was linked by the user. Defaults to the current date.
    /// </summary>
    [JsonProperty("date")]
    public DateTime? Date { get; set; }

    /// <summary>
    /// Expiry date of the credentials.
    /// </summary>
    [JsonProperty("expiry")]
    public DateTime? Expiry { get; set; }

    /// <summary>
    /// Provider-specific description of services used (eg. which accesses were granted, which database is used, ...).
    /// </summary>
    [JsonProperty("scope")]
    public string? Scope { get; set; }

    /// <summary>
    /// Endpoint of this account. For relational database connections, this corresponds to the hostname of the database.
    /// </summary>
    [JsonProperty("host")]
    public string? Host { get; set; }

    /// <summary>
    /// Indicates whether queries may be sent to this database or plugin connection. You can use this field to eg. 
    /// temporarily disable any connections / queries. This might be useful in case of elevated stress on your systems.
    /// </summary>
    [JsonProperty("active")]
    public bool Active { get; set; }

    /// <summary>
    /// Read-only. Indicates whether this connection has been disabled because the source system reported that the used credentials are invalid.
    /// </summary>
    [JsonProperty("invalid")]
    public bool Invalid { get; set; }

    /// <summary>
    /// Token or password of this account.
    /// </summary>
    [JsonProperty("token")]
    public bool Token { get; set; }

    /// <summary>
    /// Key or username of this account.
    /// </summary>
    [JsonProperty("identifier")]
    public string? Identifier { get; set; }

    /// <summary>
    /// Defaults to 0. Number of seconds queries to this data connector are cached in Cumul.io's caching layer. 
    /// Use 0 to disable caching for this connector entirely. Note that queries might still not be cached if there 
    /// are other uncached data connectors called in a query. You can invalidate the cache before the expiry time by 
    /// calling the update method on the data endpoint.
    /// </summary>
    [JsonProperty("cache")]
    public int Cache { get; set; }

    /// <summary>
    /// (Update-only) OAauth2 authorization code used to retrieve an access token and refresh token.
    /// </summary>
    [JsonProperty("code")]
    public string? Code { get; set; }
}