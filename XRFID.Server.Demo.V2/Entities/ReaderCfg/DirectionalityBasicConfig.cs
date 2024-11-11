using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

[ComplexType]
public partial class DirectionalityBasicConfig : IEquatable<DirectionalityBasicConfig?>
{
    /// <summary>
    /// Gets or Sets BeamConfig
    /// </summary>

    //// [JsonPropertyName("beamConfig")]
    //public OneOfdirectionalityBasicConfigBeamConfig? BeamConfig { get; set; }

    /// <summary>
    /// The width of the main zones (in feet)
    /// </summary>
    /// <value>The width of the main zones (in feet)</value>

    // [JsonPropertyName("innerZoneWidth")]
    public double? InnerZoneWidth { get; set; }

    /// <summary>
    /// Orientation of tag (in degrees)
    /// </summary>
    /// <value>Orientation of tag (in degrees)</value>

    // [JsonPropertyName("orientation")]
    public double? Orientation { get; set; }

    /// <summary>
    /// Height of reader (in feet)
    /// </summary>
    /// <value>Height of reader (in feet)</value>

    [Range(10, 20)]
    // [JsonPropertyName("readerHeight")]
    public double? ReaderHeight { get; set; }

    /// <summary>
    /// Height of tag (in feet)
    /// </summary>
    /// <value>Height of tag (in feet)</value>

    [Range(0, 20)]
    // [JsonPropertyName("tagHeight")]
    public double? TagHeight { get; set; }

    /// <summary>
    /// The extension of the main zone into one another (in feet)
    /// </summary>
    /// <value>The extension of the main zone into one another (in feet)</value>

    // [JsonPropertyName("zoneExtension")]
    public double? ZoneExtension { get; set; }

    /// <summary>
    /// User defined names to zones. Array must have the same number of zones as specified in the zone plan
    /// </summary>
    /// <value>User defined names to zones. Array must have the same number of zones as specified in the zone plan</value>

    // [JsonPropertyName("zoneNames")]
    public List<string>? ZoneNames { get; set; }

    // [JsonPropertyName("zonePlan")]
    public ZonePlanEnum? ZonePlan { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as DirectionalityBasicConfig);
    }

    public bool Equals(DirectionalityBasicConfig? other)
    {
        return other is not null &&
               InnerZoneWidth == other.InnerZoneWidth &&
               Orientation == other.Orientation &&
               ReaderHeight == other.ReaderHeight &&
               TagHeight == other.TagHeight &&
               ZoneExtension == other.ZoneExtension &&
               EqualityComparer<List<string>?>.Default.Equals(ZoneNames, other.ZoneNames) &&
               ZonePlan == other.ZonePlan;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(InnerZoneWidth, Orientation, ReaderHeight, TagHeight, ZoneExtension, ZoneNames, ZonePlan);
    }

    public static bool operator ==(DirectionalityBasicConfig? left, DirectionalityBasicConfig? right)
    {
        return EqualityComparer<DirectionalityBasicConfig>.Default.Equals(left, right);
    }

    public static bool operator !=(DirectionalityBasicConfig? left, DirectionalityBasicConfig? right)
    {
        return !(left == right);
    }
}