using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Common.Dto.ReaderConfiguration;

public class StartTriggerDto
{
    public StartTriggerTypeEnum StartTriggerType { get; set; }
    public GPITriggerDto StartGPITrigger { get; set; }
    public PeriodicTriggerDto StartPeriodic { get; set; }

    public StartTriggerDto()
    {
        StartGPITrigger = new GPITriggerDto();
        StartPeriodic = new PeriodicTriggerDto();
    }

    public override bool Equals(object? obj)
    {
        return obj is StartTriggerDto trigger &&
               StartTriggerType == trigger.StartTriggerType &&
               EqualityComparer<GPITriggerDto>.Default.Equals(StartGPITrigger, trigger.StartGPITrigger) &&
               EqualityComparer<PeriodicTriggerDto>.Default.Equals(StartPeriodic, trigger.StartPeriodic);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartTriggerType, StartGPITrigger, StartPeriodic);
    }
}