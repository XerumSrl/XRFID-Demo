using Xerum.XFramework.Common.Dto;
using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Common.Dto;

public class LoadingUnitDto : BaseDto
{
    public int Sequence { get; set; }
    public WorkflowType Type { get; set; }
    public string? Description { get; set; }

    public bool IsActive { get; set; }
    public bool IsValid { get; set; }
    public bool IsConsolidated { get; set; }
    public bool Sent { get; set; }

    public Guid ReaderId { get; set; }

    #region Custom attributes
    public string? Attribute1 { get; set; }
    public string? Attribute2 { get; set; }
    #endregion

    #region Data Relations

    public ReaderDto? Reader { get; set; }

    public ICollection<LoadingUnitItemDto> LoadingUnitItems { get; set; } = [];

    #endregion
}