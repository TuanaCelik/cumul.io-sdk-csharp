using Newtonsoft.Json;

namespace Cumulio.Http;

public class CumulioException : Exception
{
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("details")]
    public dynamic? Details { get; set; }

    public CumulioException()    {    }
    public CumulioException(string message):base(message) { }
    public CumulioException(string message, Exception exception):base(message, exception) { }
    public CumulioException(dynamic result)
    {
        if(result.code != null)
        {
            Code = result.code;
        }

        Details = result.details;
    }
}