namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

public class GPITrigger
{
    public bool GPIEvent { get; set; }
    public int PortNumber { get; set; }
    public uint Timeout { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is GPITrigger trigger &&
               GPIEvent == trigger.GPIEvent &&
               PortNumber == trigger.PortNumber &&
               Timeout == trigger.Timeout;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GPIEvent, PortNumber, Timeout);
    }
}