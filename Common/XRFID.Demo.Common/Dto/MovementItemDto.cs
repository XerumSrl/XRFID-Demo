using Xerum.XFramework.Common.Dto;
using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Common.Dto;

public class MovementItemDto : BaseDto
{
    public string? Description { get; set; }

    public string? SerialNumber { get; set; }

    public string Regex { get; set; } = ".*";

    public required string Epc { get; set; }
    public short Rssi { get; set; } = 0;
    public string? Tid { get; set; }
    public string? PC { get; set; }
    public int ReadsCount { get; set; } = 0;
    public bool Checked { get; set; }
    public DateTimeOffset FirstRead { get; set; }
    public DateTimeOffset LastRead { get; set; }
    public DateTimeOffset? IgnoreUntil { get; set; }
    //public DateTime Timestamp { get; set; } //see DateCreated
    public ItemStatus Status { get; set; }

    public bool IsConsolidated { get; set; }
    public bool Sent { get; set; }

    public required string ZoneName { get; set; }
    public string? PreviousZoneName { get; set; }

    public Guid MovementId { get; set; }
    public Guid? LoadingUnitItemId { get; set; }
    public Guid? ProductId { get; set; }

    #region Custom attributes
    public string? Attribute1 { get; set; }
    public string? Attribute2 { get; set; }
    #endregion

    #region Data Relations
    public MovementDto? Movement { get; set; }

    public LoadingUnitItemDto? LoadingUnitItem { get; set; }

    public ProductDto? Product { get; set; }

    #endregion
}