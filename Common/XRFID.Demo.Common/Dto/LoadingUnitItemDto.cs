using Xerum.XFramework.Common.Dto;
using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Common.Dto;

public class LoadingUnitItemDto : BaseDto
{
    public string? Description { get; set; }
    public string? SerialNumber { get; set; }

    public string Regex { get; set; } = ".*";
    public string? Epc { get; set; }
    public ItemStatus Status { get; set; }

    public bool IsConsolidated { get; set; }
    public bool Sent { get; set; }

    public Guid LoadingUnitId { get; set; }


    #region Custom attributes
    public string? Attribute1 { get; set; }
    public string? Attribute2 { get; set; }
    #endregion

    #region Data Relations

    public LoadingUnitDto? LoadingUnit { get; set; }


    public ICollection<MovementItemDto> MovementItems { get; set; } = [];

    #endregion
}