using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Common.Dto.ReaderConfiguration;

public class StopTriggerDto
{
    public StopTriggerTypeEnum StopTriggerType { get; set; }

    /// <summary>
    /// Parameter for Duration Trigger. Duration in Milli-seconds if type is STOP_TRIGGER_TYPE_DURATION.
    /// </summary>
    public uint StopTriggerDuration { get; set; }

    public GPITriggerDto StopGPITrigger { get; set; }
    public StopTriggerWithTimeoutDto StopTagObservation { get; set; }
    public StopTriggerWithTimeoutDto StopNumAttempts { get; set; }

    public StopTriggerDto()
    {
        StopTagObservation = new StopTriggerWithTimeoutDto();
        StopNumAttempts = new StopTriggerWithTimeoutDto();
        StopGPITrigger = new GPITriggerDto();
    }

    public override bool Equals(object? obj)
    {
        return obj is StopTriggerDto trigger &&
               StopTriggerType == trigger.StopTriggerType &&
               StopTriggerDuration == trigger.StopTriggerDuration &&
               EqualityComparer<GPITriggerDto>.Default.Equals(StopGPITrigger, trigger.StopGPITrigger) &&
               EqualityComparer<StopTriggerWithTimeoutDto>.Default.Equals(StopTagObservation, trigger.StopTagObservation) &&
               EqualityComparer<StopTriggerWithTimeoutDto>.Default.Equals(StopNumAttempts, trigger.StopNumAttempts);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StopTriggerType, StopTriggerDuration, StopGPITrigger, StopTagObservation, StopNumAttempts);
    }
}