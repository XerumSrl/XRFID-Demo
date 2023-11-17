namespace XRFID.Demo.Server.ViewModels;

public class PrintProductModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public string Epc { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;

    public DateTime CreationTime { get; set; } = DateTime.Now;
    public DateTime LastModificationTime { get; set; } = DateTime.Now;
}
