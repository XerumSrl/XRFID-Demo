namespace XRFID.Demo.Common.Dto.GPIOConfiguration;

public class GPIOConfigurationDto : IEquatable<GPIOConfigurationDto?>
{
    public GPIConfigurationDto? GPInSwitch { get; set; }
    public GPIConfigurationDto? GPInTrigger { get; set; }
    public GPIConfigurationDto? GPInCustom { get; set; }

    public GPOConfigurationDto? GPOutGreenLED { get; set; }
    public GPOConfigurationDto? GPOutRedLED { get; set; }
    public GPOConfigurationDto? GPOutYellowLED { get; set; }
    public GPOConfigurationDto? GPOutBuzzer { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as GPIOConfigurationDto);
    }

    public bool Equals(GPIOConfigurationDto? other)
    {
        return other is not null &&
               EqualityComparer<GPIConfigurationDto?>.Default.Equals(GPInSwitch, other.GPInSwitch) &&
               EqualityComparer<GPIConfigurationDto?>.Default.Equals(GPInTrigger, other.GPInTrigger) &&
               EqualityComparer<GPIConfigurationDto?>.Default.Equals(GPInCustom, other.GPInCustom) &&
               EqualityComparer<GPOConfigurationDto?>.Default.Equals(GPOutGreenLED, other.GPOutGreenLED) &&
               EqualityComparer<GPOConfigurationDto?>.Default.Equals(GPOutRedLED, other.GPOutRedLED) &&
               EqualityComparer<GPOConfigurationDto?>.Default.Equals(GPOutYellowLED, other.GPOutYellowLED) &&
               EqualityComparer<GPOConfigurationDto?>.Default.Equals(GPOutBuzzer, other.GPOutBuzzer);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GPInSwitch, GPInTrigger, GPInCustom, GPOutGreenLED, GPOutRedLED, GPOutYellowLED, GPOutBuzzer);
    }

    public static bool operator ==(GPIOConfigurationDto? left, GPIOConfigurationDto? right)
    {
        return EqualityComparer<GPIOConfigurationDto>.Default.Equals(left, right);
    }

    public static bool operator !=(GPIOConfigurationDto? left, GPIOConfigurationDto? right)
    {
        return !(left == right);
    }
}