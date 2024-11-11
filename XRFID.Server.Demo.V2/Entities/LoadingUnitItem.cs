using Xerum.XFramework.Common.Enums;
using Xerum.XFramework.DBAccess.Entities;

namespace XRFID.Server.Demo.V2.Entities;

public class LoadingUnitItem : AuditEntity
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

    #region EF Logic Links

#pragma warning disable CS8618 // not nullable because from non nullable foreign key
    public LoadingUnit LoadingUnit { get; set; }
#pragma warning restore CS8618 // not nullable because from non nullable foreign key


    public ICollection<MovementItem> MovementItems { get; set; } = [];

    #endregion
}