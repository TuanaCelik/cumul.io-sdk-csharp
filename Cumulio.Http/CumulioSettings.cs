namespace Cumulio.Http;

public class CumulioSettings
{
    public string APIKey { get; set; } = string.Empty;
    public string APIToken { get; set; } = string.Empty;
    public string APIVersion { get; set; } = "0.1.0";
    public string Port { get; set; } = "443";
    public string Host { get; set; } = string.Empty;
}