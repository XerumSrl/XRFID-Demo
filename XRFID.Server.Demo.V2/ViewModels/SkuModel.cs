namespace XRFID.Server.Demo.V2.ViewModels;

public class SkuModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTimeOffset? DateCreated { get; set; }

    public DateTimeOffset? EffectivityStart { get; set; }
    public DateTimeOffset? EffectivityEnd { get; set; }

    public List<ProductModel> Products { get; set; } = new List<ProductModel>();
}