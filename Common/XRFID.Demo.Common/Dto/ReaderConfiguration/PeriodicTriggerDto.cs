namespace XRFID.Demo.Common.Dto.ReaderConfiguration;

public class PeriodicTriggerDto
{
    public uint Period { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is PeriodicTriggerDto trigger &&
               StartTime == trigger.StartTime &&
               Period == trigger.Period;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartTime, Period);
    }
}