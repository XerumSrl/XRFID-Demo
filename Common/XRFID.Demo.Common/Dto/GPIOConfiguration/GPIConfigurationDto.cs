using System.Text.Json.Serialization;

namespace XRFID.Demo.Common.Dto.GPIOConfiguration;

public class GPIConfigurationDto : IEquatable<GPIConfigurationDto?>
{
    public int Pin { get; set; }

    public bool LogicOn { get; set; } = true;

    [JsonIgnore]
    public bool LogicOff { get => !LogicOn; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as GPIConfigurationDto);
    }

    public bool Equals(GPIConfigurationDto? other)
    {
        return other is not null &&
               Pin == other.Pin &&
               LogicOn == other.LogicOn;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Pin, LogicOn);
    }

    public static bool operator ==(GPIConfigurationDto? left, GPIConfigurationDto? right)
    {
        return EqualityComparer<GPIConfigurationDto>.Default.Equals(left, right);
    }

    public static bool operator !=(GPIConfigurationDto? left, GPIConfigurationDto? right)
    {
        return !(left == right);
    }
}