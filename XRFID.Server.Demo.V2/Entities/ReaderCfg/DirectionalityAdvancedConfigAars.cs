using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

[ComplexType]
public partial class DirectionalityAdvancedConfigAars : IEquatable<DirectionalityAdvancedConfigAars?>
{
    /// <summary>
    /// AAR IP Address and Port to connect to (e.g. 192.168.0.23:5160)
    /// </summary>
    /// <value>AAR IP Address and Port to connect to (e.g. 192.168.0.23:5160)</value>
    // [JsonPropertyName("address")]
    public string? Address { get; set; }

    /// <summary>
    /// X Location (in feet) of ATR relative to other ATR
    /// </summary>
    /// <value>X Location (in feet) of ATR relative to other ATR</value>
    // [JsonPropertyName("x")]
    public double? X { get; set; }

    /// <summary>
    /// Y Location (in feet) of ATR relative to other ATR
    /// </summary>
    /// <value>Y Location (in feet) of ATR relative to other ATR</value>
    // [JsonPropertyName("y")]
    public double? Y { get; set; }

    /// <summary>
    /// Height (in feet) of ATR 
    /// </summary>
    /// <value>Height (in feet) of ATR </value>
    // [JsonPropertyName("z")]
    public double? Z { get; set; }

    /// <summary>
    /// Degrees of offset of ATR from North
    /// </summary>
    /// <value>Degrees of offset of ATR from North</value>
    [Range(0, 359)]
    // [JsonPropertyName("offset")]
    public double? Offset { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as DirectionalityAdvancedConfigAars);
    }

    public bool Equals(DirectionalityAdvancedConfigAars? other)
    {
        return other is not null &&
               Address == other.Address &&
               X == other.X &&
               Y == other.Y &&
               Z == other.Z &&
               Offset == other.Offset;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Address, X, Y, Z, Offset);
    }

    public static bool operator ==(DirectionalityAdvancedConfigAars? left, DirectionalityAdvancedConfigAars? right)
    {
        return EqualityComparer<DirectionalityAdvancedConfigAars>.Default.Equals(left, right);
    }

    public static bool operator !=(DirectionalityAdvancedConfigAars? left, DirectionalityAdvancedConfigAars? right)
    {
        return !(left == right);
    }
}
