namespace XRFID.Demo.Common.Dto.ReaderConfiguration;

public class GPITriggerDto
{
    public bool GPIEvent { get; set; }
    public int PortNumber { get; set; }
    public uint Timeout { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is GPITriggerDto trigger &&
               GPIEvent == trigger.GPIEvent &&
               PortNumber == trigger.PortNumber &&
               Timeout == trigger.Timeout;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GPIEvent, PortNumber, Timeout);
    }
}