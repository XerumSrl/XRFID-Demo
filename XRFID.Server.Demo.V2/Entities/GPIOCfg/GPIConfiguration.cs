using System.Text.Json.Serialization;

namespace XRFID.Server.Demo.V2.Entities.GPIOCfg;

public class GPIConfiguration : IEquatable<GPIConfiguration?>
{
    public int Pin { get; set; }

    public bool LogicOn { get; set; } = true;

    [JsonIgnore]
    public bool LogicOff { get => !LogicOn; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as GPIConfiguration);
    }

    public bool Equals(GPIConfiguration? other)
    {
        return other is not null &&
               Pin == other.Pin &&
               LogicOn == other.LogicOn;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Pin, LogicOn);
    }

    public static bool operator ==(GPIConfiguration? left, GPIConfiguration? right)
    {
        return EqualityComparer<GPIConfiguration>.Default.Equals(left, right);
    }

    public static bool operator !=(GPIConfiguration? left, GPIConfiguration? right)
    {
        return !(left == right);
    }
}