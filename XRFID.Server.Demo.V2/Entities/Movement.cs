﻿using Xerum.XFramework.Common.Enums;
using Xerum.XFramework.DBAccess.Entities;
using XRFID.Demo.Common.Enumerations;

namespace XRFID.Server.Demo.V2.Entities;

public class Movement : AuditEntity
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

    #region Custom attributes
    public string? Attribute1 { get; set; }
    public string? Attribute2 { get; set; }
    #endregion

    #region EF Logic Links

    public Reader? Reader { get; set; }

    public Printer? Printer { get; set; }

    public MovementDirection Direction { get; set; }

    public ICollection<MovementItem> MovementItems { get; set; } = [];

    public ICollection<LoadingUnit> LoadingUnits { get; set; } = [];

    #endregion
}
