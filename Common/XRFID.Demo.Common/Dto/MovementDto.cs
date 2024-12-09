using Xerum.XFramework.Common.Dto;
using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Common.Dto;

public class MovementDto : BaseDto
{
    public int Sequence { get; set; }
    public WorkflowType Type { get; set; }
    public MovementContentEnum ContentType { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public bool IsValid { get; set; } = false;
    public bool UnexpectedItem { get; set; } = false;
    public bool MissingItem { get; set; } = false;
    public bool OverflowItem { get; set; } = false;
    public bool IsActive { get; set; } = false;
    public bool IsConsolidated { get; set; } = false;
    public bool Sent { get; set; } = false;

    public Guid? ReaderId { get; set; }

    public Guid? PrinterId { get; set; }

    public Guid? BOMId { get; set; }

    #region Custom attributes
    public string? Attribute1 { get; set; }
    public string? Attribute2 { get; set; }
    #endregion

    #region Data Relations

    public ReaderDto? Reader { get; set; }

    public PrinterDto? Printer { get; set; }

    public ICollection<MovementItemDto> MovementItems { get; set; } = [];

    public ICollection<LoadingUnitDto> LoadingUnits { get; set; } = [];

    #endregion
}
