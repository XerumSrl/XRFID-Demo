using System.Text.Json.Serialization;

namespace XRFID.Demo.Modules.Mqtt.Contracts;
public class RawDirectionalityTagPayload
{
    [JsonPropertyName("azimuth")]
    public float Azimuth { get; set; } = 0;

    [JsonPropertyName("azimuthConf")]
    public int AzimuthConfidentiality { get; set; } = 0;

    [JsonPropertyName("elevation")]
    public float Elevation { get; set; } = 0;

    [JsonPropertyName("elevationConf")]
    public int ElevationConfidentiality { get; set; } = 0;

    [JsonPropertyName("format")]
    public string? Format { get; set; }

    [JsonPropertyName("idHex")]
    public required string IdHex { get; set; }

}
