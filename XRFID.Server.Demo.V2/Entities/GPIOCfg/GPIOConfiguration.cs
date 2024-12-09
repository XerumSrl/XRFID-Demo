namespace XRFID.Server.Demo.V2.Entities.GPIOCfg;

public class GPIOConfiguration : IEquatable<GPIOConfiguration?>
{
    public GPIConfiguration? GPInSwitch { get; set; }
    public GPIConfiguration? GPInTrigger { get; set; }
    public GPIConfiguration? GPInCustom { get; set; }


    public GPOConfiguration? GPOutGreenLED { get; set; }
    public GPOConfiguration? GPOutRedLED { get; set; }
    public GPOConfiguration? GPOutYellowLED { get; set; }
    public GPOConfiguration? GPOutBuzzer { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as GPIOConfiguration);
    }

    public bool Equals(GPIOConfiguration? other)
    {
        return other is not null &&
               EqualityComparer<GPIConfiguration?>.Default.Equals(GPInSwitch, other.GPInSwitch) &&
               EqualityComparer<GPIConfiguration?>.Default.Equals(GPInTrigger, other.GPInTrigger) &&
               EqualityComparer<GPIConfiguration?>.Default.Equals(GPInCustom, other.GPInCustom) &&
               EqualityComparer<GPOConfiguration?>.Default.Equals(GPOutGreenLED, other.GPOutGreenLED) &&
               EqualityComparer<GPOConfiguration?>.Default.Equals(GPOutRedLED, other.GPOutRedLED) &&
               EqualityComparer<GPOConfiguration?>.Default.Equals(GPOutYellowLED, other.GPOutYellowLED) &&
               EqualityComparer<GPOConfiguration?>.Default.Equals(GPOutBuzzer, other.GPOutBuzzer);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GPInSwitch, GPInTrigger, GPInCustom, GPOutGreenLED, GPOutRedLED, GPOutYellowLED, GPOutBuzzer);
    }

    public static bool operator ==(GPIOConfiguration? left, GPIOConfiguration? right)
    {
        return EqualityComparer<GPIOConfiguration>.Default.Equals(left, right);
    }

    public static bool operator !=(GPIOConfiguration? left, GPIOConfiguration? right)
    {
        return !(left == right);
    }
}