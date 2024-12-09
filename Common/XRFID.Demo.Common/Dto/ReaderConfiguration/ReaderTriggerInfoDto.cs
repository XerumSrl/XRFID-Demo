namespace XRFID.Demo.Common.Dto.ReaderConfiguration;

public class ReaderTriggerInfoDto
{
    /// <summary>
    /// Condition to be met to stop the operation.
    /// </summary>
    public StartTriggerDto StartTrigger { get; set; }

    /// <summary>
    /// Condition to be met to stop the operation.
    /// </summary>
    public StopTriggerDto StopTrigger { get; set; }

    /// <summary>
    /// Enable Tag Event Report
    /// </summary>
    public bool EnableTagEventReport { get; set; }

    /// <summary>
    /// Specifies the configuration of events for the reader to report tag state changes.
    /// </summary>
    public TagEventReportInfoDto TagEventReportInfo { get; set; }

    /// <summary>
    /// Possible Values: 
    /// 0 - Tag reports will be triggered when the Stop-Criteria is met. In case of periodic trigger, reports will be triggered first time the tag is seen and if the tag is continued to be read after the period has elapsed . 
    /// n - Report when 'n' unique tags are found The default settings is 1, which implies to get Tag reports immediately
    /// </summary>
    public uint TagReportTrigger { get; set; }
    /// <summary>
    /// Reporting triggers e.g. period based reporting of tags ReportTriggers.
    /// ReportTriggersPeriod - Duration in seconds after which the tag will be reported if the tag is continued to be read after the period 
    /// ReportTriggersPeriod and ReportTriggers are mutually exclusive and ReportTriggersPeriod have priority on tagReportTrigger
    /// </summary>
    public uint ReportTriggersPeriod { get; set; }


    public ReaderTriggerInfoDto()
    {
        StartTrigger = new StartTriggerDto();
        StopTrigger = new StopTriggerDto();
        TagEventReportInfo = new TagEventReportInfoDto();
        TagReportTrigger = 1u;
        EnableTagEventReport = false;
        ReportTriggersPeriod = 0u;

    }

    public override bool Equals(object? obj)
    {
        return obj is ReaderTriggerInfoDto info &&
               EqualityComparer<StartTriggerDto>.Default.Equals(StartTrigger, info.StartTrigger) &&
               EqualityComparer<StopTriggerDto>.Default.Equals(StopTrigger, info.StopTrigger) &&
               EnableTagEventReport == info.EnableTagEventReport &&
               EqualityComparer<TagEventReportInfoDto>.Default.Equals(TagEventReportInfo, info.TagEventReportInfo) &&
               TagReportTrigger == info.TagReportTrigger &&
               ReportTriggersPeriod == info.ReportTriggersPeriod;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(StartTrigger, StopTrigger, EnableTagEventReport, TagEventReportInfo, TagReportTrigger, ReportTriggersPeriod);
    }
}