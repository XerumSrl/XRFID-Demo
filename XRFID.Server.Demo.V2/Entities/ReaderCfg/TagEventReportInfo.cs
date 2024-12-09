using Xerum.XFramework.Common.Enums;

namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

public class TagEventReportInfo
{
    public TagEventReportTriggerEnum newTagEvent;
    public ushort newTagEventModeratedTimeoutMilliseconds;
    public TagEventReportTriggerEnum reportTagInvisibleEvent;
    public ushort tagInvisibleEventModeratedTimeoutMilliseconds;
    public TagEventReportTriggerEnum reportTagBackToVisibilityEvent;
    public ushort tagBackToVisibilityModeratedTimeoutMilliseconds;
    public TagMovingEventReportEnum reportTagMovingEvent;
    public ushort tagStationaryModeratedTimeoutMilliseconds;
}