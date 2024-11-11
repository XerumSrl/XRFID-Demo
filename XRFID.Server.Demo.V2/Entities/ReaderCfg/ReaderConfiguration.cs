using Xerum.XFramework.Common.Enums;

namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

public class ReaderConfiguration : IEquatable<ReaderConfiguration?>
{
    public string ReaderName { get; set; } = "Default Reader";
    public string ReaderIp { get; set; } = "127.0.0.1";
    public string ReaderPort { get; set; } = "0";
    public RfidManufacturers ReaderManufacturer { get; set; } = RfidManufacturers.Zebra;

    //public ReaderTriggerInfo? TriggerInfo { get; set; }
    public required WorkflowSettings WorkflowSettings { get; set; }

    public required ModeSpecificSettings? ModeSpecificSettings { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ReaderConfiguration);
    }

    public bool Equals(ReaderConfiguration? other)
    {
        return other is not null &&
               ReaderName == other.ReaderName &&
               ReaderIp == other.ReaderIp &&
               ReaderPort == other.ReaderPort &&
               ReaderManufacturer == other.ReaderManufacturer &&
               EqualityComparer<WorkflowSettings>.Default.Equals(WorkflowSettings, other.WorkflowSettings) &&
               EqualityComparer<ModeSpecificSettings?>.Default.Equals(ModeSpecificSettings, other.ModeSpecificSettings);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ReaderName, ReaderIp, ReaderPort, ReaderManufacturer, WorkflowSettings, ModeSpecificSettings);
    }

    public static bool operator ==(ReaderConfiguration? left, ReaderConfiguration? right)
    {
        return EqualityComparer<ReaderConfiguration>.Default.Equals(left, right);
    }

    public static bool operator !=(ReaderConfiguration? left, ReaderConfiguration? right)
    {
        return !(left == right);
    }
}
