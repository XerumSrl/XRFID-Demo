using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Common.Dto.ReaderConfiguration;

public class TagEventReportInfoDto
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