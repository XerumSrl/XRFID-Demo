using Xerum.XFramework.Common.Enums;

namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

public class StopTrigger
{
    public StopTriggerTypeEnum StopTriggerType { get; set; }

    /// <summary>
    /// Parameter for Duration Trigger. Duration in Milli-seconds if type is STOP_TRIGGER_TYPE_DURATION.
    /// </summary>
    public uint StopTriggerDuration { get; set; }

    public GPITrigger StopGPITrigger { get; set; }
    public StopTriggerWithTimeout StopTagObservation { get; set; }
    public StopTriggerWithTimeout StopNumAttempts { get; set; }

    public StopTrigger()
    {
        StopTagObservation = new StopTriggerWithTimeout();
        StopNumAttempts = new StopTriggerWithTimeout();
        StopGPITrigger = new GPITrigger();
    }

    public override bool Equals(object? obj)
    {
        return obj is StopTrigger trigger &&
               StopTriggerType == trigger.StopTriggerType &&
               StopTriggerDuration == trigger.StopTriggerDuration &&
               EqualityComparer<GPITrigger>.Default.Equals(StopGPITrigger, trigger.StopGPITrigger) &&
               EqualityComparer<StopTriggerWithTimeout>.Default.Equals(StopTagObservation, trigger.StopTagObservation) &&
               EqualityComparer<StopTriggerWithTimeout>.Default.Equals(StopNumAttempts, trigger.StopNumAttempts);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StopTriggerType, StopTriggerDuration, StopGPITrigger, StopTagObservation, StopNumAttempts);
    }
}