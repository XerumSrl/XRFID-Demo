using XRFID.Demo.Common.Enumerations;
using XRFID.Server.Demo.V2.ViewModels.Enums;

namespace XRFID.Server.Demo.V2.ViewModels;

public class CheckItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Epc { get; set; } = string.Empty;
    public CheckStatusEnum CheckStatus { get; set; } = CheckStatusEnum.NotFound;
    public string Description { get; set; } = string.Empty;
    public DateTime DateTime { get; set; } = DateTime.Now;
    public MovementDirection Direction { get; set; }
    public string ZoneName { get; set; }
    public string PrevZoneName { get; set; }
}
