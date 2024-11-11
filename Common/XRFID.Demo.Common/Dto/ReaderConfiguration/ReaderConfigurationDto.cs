using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Common.Dto.ReaderConfiguration;

public class ReaderConfigurationDto : IEquatable<ReaderConfigurationDto?>
{
    public string ReaderName { get; set; } = "Default Reader";
    public string ReaderIp { get; set; } = "127.0.0.1";
    public string ReaderPort { get; set; } = "0";
    public RfidManufacturers ReaderManufacturer { get; set; } = RfidManufacturers.Zebra;

    public required WorkflowSettingsDto WorkflowSettings { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ReaderConfigurationDto);
    }

    public bool Equals(ReaderConfigurationDto? other)
    {
        return other is not null &&
               ReaderName == other.ReaderName &&
               ReaderIp == other.ReaderIp &&
               ReaderPort == other.ReaderPort &&
               ReaderManufacturer == other.ReaderManufacturer &&
               EqualityComparer<WorkflowSettingsDto>.Default.Equals(WorkflowSettings, other.WorkflowSettings);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ReaderName, ReaderIp, ReaderPort, ReaderManufacturer, WorkflowSettings);
    }

    public static bool operator ==(ReaderConfigurationDto? left, ReaderConfigurationDto? right)
    {
        return EqualityComparer<ReaderConfigurationDto>.Default.Equals(left, right);
    }

    public static bool operator !=(ReaderConfigurationDto? left, ReaderConfigurationDto? right)
    {
        return !(left == right);
    }
}
