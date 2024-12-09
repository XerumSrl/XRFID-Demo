using Xerum.XFramework.Common.Enums;
using Xerum.XFramework.DBAccess.Entities;

namespace XRFID.Server.Demo.V2.Entities;

public class MovementItem : AuditEntity
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

    #region EF Logic Links

#pragma warning disable CS8618 // not nullable because from non nullable foreign key
    public Movement Movement { get; set; }
#pragma warning restore CS8618 // not nullable because from non nullable foreign key

    public LoadingUnitItem? LoadingUnitItem { get; set; }

    public Product? Product { get; set; }

    #endregion
}