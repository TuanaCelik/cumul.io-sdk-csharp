namespace Cumulio.Blazor;

public class CumulioDashboardProperties
{
    public string? DashboardId { get; set; }
    public string? DashboardSlug { get; set; }
    public string? AuthKey { get; set; }
    public string? AuthToken { get; set; }
    public CumulioScreenMode? ScreenMode { get; set; }
    public CumulioLanguage? Language { get; set; }

    /// <summary>
    /// The timezone you you wish to use in your dashboard. This timezone id needs to be a valid id that is 
    /// available in the IANA timezone database, for example: Europe/Brussels or America/New_York.
    /// https://en.wikipedia.org/wiki/List_of_tz_database_time_zones#List
    /// </summary>
    public string? TimeZoneId { get; set; }

    public string? LoaderBackground { get; set; }
    public string? LoaderFontColor { get; set; }
    public string? LoaderSpinnerColor { get; set; }
    public string? LoaderSpinnerBackground { get; set; }
    public string? ItemId { get; set; }
    public ItemDimension ItemDimensions { get; set; }
}

public class ItemDimension
{
    public string? Width { get; set; }
    public string? Height { get; set; }
}

public enum CumulioScreenMode
{
    Auto,
    Fixed,
    Mobile,
    Tablet,
    Desktop,
    LargeScreen
}
public enum CumulioLanguage
{
    EN,
    CS,
    DA,
    DE,
    ES,
    FI,
    FR,
    HE,
    HU,
    IT,
    JA,
    KO,
    MK,
    NL,
    PL,
    PT,
    RU,
    SV,
    ZH_CN,
    ZH_TW
}