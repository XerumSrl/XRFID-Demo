using System.Runtime.Serialization;
using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Modules.Mqtt.Contracts;

[DataContract]
public class ZebraDirectionalityTagData
{
    public required DateTimeOffset Timestamp { get; set; } = DateTime.Now;

    public required string ReaderName { get; set; }

    public int? EventNum { get; set; }

    public required string IdHex { get; set; }

    public required TagStateEnum State { get; set; }

    public required short Zone { get; set; }

    public string? ZoneName { get; set; }

    public decimal? PrevZone { get; set; }

    public string? PrevZoneName { get; set; }

    public TagDirectionEnum? Direction { get; set; }

    public List<List<decimal>>? ZoneHistory { get; set; }

    public List<List<decimal>>? LocationHistory { get; set; }

    public string? UserDefined { get; set; }

    public DateTimeOffset? LastSeenTimestamp { get; set; }
}
