using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace XRFID.Demo.Common.Dto.ReaderConfiguration;

public class TagFoundFilterDto : IEquatable<TagFoundFilterDto?>
{
    public int TagFoundMinReadCycles { get; set; }
    public int TagFoundMinReadCyclesWeight { get; set; } = 1;
    public int MinReadsCount { get; set; }
    public int MinReadsCountWeight { get; set; } = 1;
    public int MinRssi { get; set; }
    public int MinRssiWeight { get; set; } = 1;
    public int TimeSpanWeight { get; set; } = 1;
    public int TagReportIntervalSeconds { get; set; } = 1;
    public double MinimumScore { get; set; }

    [JsonIgnore]
    [XmlIgnore]
    public long TagReportInterval
    {
        get => TagReportIntervalSeconds * 1000;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TagFoundFilterDto);
    }

    public bool Equals(TagFoundFilterDto? other)
    {
        return other is not null &&
               TagFoundMinReadCycles == other.TagFoundMinReadCycles &&
               TagFoundMinReadCyclesWeight == other.TagFoundMinReadCyclesWeight &&
               MinReadsCount == other.MinReadsCount &&
               MinReadsCountWeight == other.MinReadsCountWeight &&
               MinRssi == other.MinRssi &&
               MinRssiWeight == other.MinRssiWeight &&
               TimeSpanWeight == other.TimeSpanWeight &&
               TagReportIntervalSeconds == other.TagReportIntervalSeconds &&
               MinimumScore == other.MinimumScore;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(TagFoundMinReadCycles);
        hash.Add(TagFoundMinReadCyclesWeight);
        hash.Add(MinReadsCount);
        hash.Add(MinReadsCountWeight);
        hash.Add(MinRssi);
        hash.Add(MinRssiWeight);
        hash.Add(TimeSpanWeight);
        hash.Add(TagReportIntervalSeconds);
        hash.Add(MinimumScore);
        return hash.ToHashCode();
    }

    public static bool operator ==(TagFoundFilterDto? left, TagFoundFilterDto? right)
    {
        return EqualityComparer<TagFoundFilterDto>.Default.Equals(left, right);
    }

    public static bool operator !=(TagFoundFilterDto? left, TagFoundFilterDto? right)
    {
        return !(left == right);
    }
}