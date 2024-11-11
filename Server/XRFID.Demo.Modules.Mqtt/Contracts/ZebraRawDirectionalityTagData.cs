using System.Runtime.Serialization;

namespace XRFID.Demo.Modules.Mqtt.Contracts;

[DataContract]
public class ZebraRawDirectionalityTagData
{
    public required DateTimeOffset Timestamp { get; set; } = DateTime.Now;
    public required string ReaderName { get; set; }
    public float Azimuth { get; set; }
    public int AzimuthConfidentiality { get; set; }
    public float Elevation { get; set; }
    public int ElevationConfidentiality { get; set; }
    public string? Format { get; set; }

    public required string IdHex { get; set; }
}
