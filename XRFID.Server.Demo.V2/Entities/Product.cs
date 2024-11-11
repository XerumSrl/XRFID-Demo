using Xerum.XFramework.Common.Enums;
using Xerum.XFramework.DBAccess.Entities;

namespace XRFID.Server.Demo.V2.Entities;

public class Product : AuditEntity
{
    public string? Description { get; set; }

    public string? Type { get; set; }

    public string? Note { get; set; }

    public string? BatchReference { get; set; }

    public string? Attrib1 { get; set; }

    public string? Attrib2 { get; set; }

    public string? Attrib3 { get; set; }

    public Guid? SkuId { get; set; }

    public string? SkuReference { get; set; }

    public ItemStatus Status { get; set; } = ItemStatus.None;

    public string? Epc { get; set; }

    public string Regex { get; set; } = ".*";

    public string? SerialNumber { get; set; }

    public string? Barcode { get; set; }

    public int ContentQuantity { get; set; } = 1;

    public string? ContentUom { get; set; }

    public string? ContentQuality { get; set; }

    //soft delete
    public bool IsDeleted { get; set; } = false;

    #region EF Logic Links

    public Sku? Sku { get; set; }

    public ICollection<MovementItem> MovementItems { get; set; } = [];

    #endregion
}