namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

public class PeriodicTrigger
{
    public uint Period { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is PeriodicTrigger trigger &&
               StartTime == trigger.StartTime &&
               Period == trigger.Period;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartTime, Period);
    }
}