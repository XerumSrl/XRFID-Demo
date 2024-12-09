using Xerum.XFramework.DBAccess.Entities;

namespace XRFID.Server.Demo.V2.Entities;

public class Sku : AuditEntity
{
    public string? Description { get; set; }

    public string? Type { get; set; }


    public string? Uom { get; set; }

    public string? Weight { get; set; }


    public string? Note { get; set; }

    public string? Model { get; set; }

    public string? Part { get; set; }

    public string? Color { get; set; }

    public string? Size { get; set; }

    public string? Drop { get; set; }



    public string? Attrib1 { get; set; }

    public string? Attrib2 { get; set; }

    public string? Attrib3 { get; set; }

    public DateTimeOffset? EffectivityStart { get; set; }
    public DateTimeOffset? EffectivityEnd { get; set; }

    public Guid? OldSkuId { get; set; }

    #region EF Logic Links

    public Sku? OldSku { get; set; }

    public Sku? NewSku { get; set; }

    public ICollection<Product> Products { get; set; } = [];

    #endregion
}