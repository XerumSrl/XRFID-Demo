using Xerum.XFramework.Common.Enums;
using Xerum.XFramework.DBAccess.Entities;

namespace XRFID.Server.Demo.V2.Entities;

public class LoadingUnit : AuditEntity
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

    #region EF Logic Links

#pragma warning disable CS8618 // not nullable because from non nullable foreign key
    public Reader Reader { get; set; }
#pragma warning restore CS8618 

    public ICollection<LoadingUnitItem> LoadingUnitItems { get; set; } = [];

    #endregion
}