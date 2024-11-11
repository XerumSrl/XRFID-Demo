using System.Text.Json.Serialization;

namespace XRFID.Server.Demo.V2.Entities.GPIOCfg;

public class GPOConfiguration : IEquatable<GPOConfiguration?>
{
    public int Pin { get; set; }

    public bool LogicOn { get; set; } = true;

    [JsonIgnore]
    public bool LogicOff { get => !LogicOn; }

    public int Duration { get; set; } = 0;

    public int Frequence { get; set; } = 0;

    public override bool Equals(object? obj)
    {
        return Equals(obj as GPOConfiguration);
    }

    public bool Equals(GPOConfiguration? other)
    {
        return other is not null &&
               Pin == other.Pin &&
               LogicOn == other.LogicOn &&
               Duration == other.Duration &&
               Frequence == other.Frequence;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Pin, LogicOn, Duration, Frequence);
    }

    public static bool operator ==(GPOConfiguration? left, GPOConfiguration? right)
    {
        return EqualityComparer<GPOConfiguration>.Default.Equals(left, right);
    }

    public static bool operator !=(GPOConfiguration? left, GPOConfiguration? right)
    {
        return !(left == right);
    }
}