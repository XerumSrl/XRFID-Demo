namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

public class TagFoundFilter
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

    public override bool Equals(object? obj)
    {
        return obj is TagFoundFilter filter &&
               TagFoundMinReadCycles == filter.TagFoundMinReadCycles &&
               TagFoundMinReadCyclesWeight == filter.TagFoundMinReadCyclesWeight &&
               MinReadsCount == filter.MinReadsCount &&
               MinReadsCountWeight == filter.MinReadsCountWeight &&
               MinRssi == filter.MinRssi &&
               MinRssiWeight == filter.MinRssiWeight &&
               TimeSpanWeight == filter.TimeSpanWeight &&
               TagReportIntervalSeconds == filter.TagReportIntervalSeconds &&
               MinimumScore == filter.MinimumScore;
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
}