using System.Text.Json.Serialization;

namespace XRFID.Demo.Common.Dto.GPIOConfiguration;

public class GPOConfigurationDto : IEquatable<GPOConfigurationDto?>
{
    public int Pin { get; set; }

    public bool LogicOn { get; set; } = true;

    [JsonIgnore]
    public bool LogicOff { get => !LogicOn; }

    public int Duration { get; set; } = 0;

    public int Frequence { get; set; } = 0;

    public override bool Equals(object? obj)
    {
        return Equals(obj as GPOConfigurationDto);
    }

    public bool Equals(GPOConfigurationDto? other)
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

    public static bool operator ==(GPOConfigurationDto? left, GPOConfigurationDto? right)
    {
        return EqualityComparer<GPOConfigurationDto>.Default.Equals(left, right);
    }

    public static bool operator !=(GPOConfigurationDto? left, GPOConfigurationDto? right)
    {
        return !(left == right);
    }
}