using Xerum.XFramework.Common.Dto;
using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Common.Dto;

public class LabelDto : BaseDto
{
    public int Version { get; set; } = 0;
    public string? Description { get; set; }
    public string? Content { get; set; }
    public PrinterLanguage Language { get; set; } = PrinterLanguage.ZPL;
    public bool IsActive { get; set; } = true;
}
