using Xerum.XFramework.Common.Enums;
using Xerum.XFramework.DBAccess.Entities;

namespace XRFID.Server.Demo.V2.Entities;

public class Label : AuditEntity
{
    public int Version { get; set; } = 0;
    public string? Description { get; set; }
    public string? Content { get; set; }
    public PrinterLanguage Language { get; set; } = PrinterLanguage.ZPL;
    public bool IsActive { get; set; } = true;
}
