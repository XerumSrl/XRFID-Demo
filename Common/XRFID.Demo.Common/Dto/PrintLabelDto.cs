namespace XRFID.Demo.Common.Dto;
public class PrintLabelDto
{
    public Guid PrinterId { get; set; }
    public string? PrinterName { get; set; }
    public Guid LabelId { get; set; }
    public string? LabelName { get; set; }
    public int LabelQuantity { get; set; } = 1;
    public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
}
