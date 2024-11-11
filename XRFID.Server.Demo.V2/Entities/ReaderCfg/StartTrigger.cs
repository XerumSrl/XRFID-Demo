using Xerum.XFramework.Common.Enums;

namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

public class StartTrigger
{
    public StartTriggerTypeEnum StartTriggerType { get; set; }
    public GPITrigger StartGPITrigger { get; set; }
    public PeriodicTrigger StartPeriodic { get; set; }

    public StartTrigger()
    {
        StartGPITrigger = new GPITrigger();
        StartPeriodic = new PeriodicTrigger();
    }

    public override bool Equals(object? obj)
    {
        return obj is StartTrigger trigger &&
               StartTriggerType == trigger.StartTriggerType &&
               EqualityComparer<GPITrigger>.Default.Equals(StartGPITrigger, trigger.StartGPITrigger) &&
               EqualityComparer<PeriodicTrigger>.Default.Equals(StartPeriodic, trigger.StartPeriodic);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartTriggerType, StartGPITrigger, StartPeriodic);
    }
}