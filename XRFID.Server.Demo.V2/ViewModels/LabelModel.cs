using Xerum.XFramework.Common.Enums;

namespace XRFID.Server.Demo.V2.ViewModels;

public class LabelModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public int Version { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public PrinterLanguage Language { get; set; }
    public bool IsActive { get; set; }

}
