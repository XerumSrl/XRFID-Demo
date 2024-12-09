using System.ComponentModel.DataAnnotations.Schema;

namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

[ComplexType]
public class ModeSpecificSettings : IEquatable<ModeSpecificSettings?>
{
    public ModeSpecificSettings()
    {
        AdvancedConfig = new();
        BasicConfig = new();
    }

    /// <summary>
    /// Gets or Sets AdvancedConfig
    /// </summary>
    // [JsonPropertyName("advancedConfig")]
    public DirectionalityAdvancedConfig? AdvancedConfig { get; set; }

    /// <summary>
    /// Gets or Sets BasicConfig
    /// </summary>
    // [JsonPropertyName("basicConfig")]
    public DirectionalityBasicConfig? BasicConfig { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ModeSpecificSettings);
    }

    public bool Equals(ModeSpecificSettings? other)
    {
        return other is not null &&
               EqualityComparer<DirectionalityAdvancedConfig?>.Default.Equals(AdvancedConfig, other.AdvancedConfig) &&
               EqualityComparer<DirectionalityBasicConfig?>.Default.Equals(BasicConfig, other.BasicConfig);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AdvancedConfig, BasicConfig);
    }

    public static bool operator ==(ModeSpecificSettings? left, ModeSpecificSettings? right)
    {
        return EqualityComparer<ModeSpecificSettings>.Default.Equals(left, right);
    }

    public static bool operator !=(ModeSpecificSettings? left, ModeSpecificSettings? right)
    {
        return !(left == right);
    }
}
