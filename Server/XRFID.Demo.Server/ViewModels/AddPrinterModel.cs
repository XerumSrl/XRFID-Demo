using System.ComponentModel.DataAnnotations;
using Xerum.XFramework.Common.Enums;

namespace XRFID.Demo.Server.ViewModels;

public class AddPrinterModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string Reference { get; set; } = string.Empty;

    [Required]
    [RegularExpression(pattern: "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", ErrorMessage = "Please enter a valid IPV4.")]
    public string Ip { get; set; } = "127.0.0.1";

    [Required]
    [Range(1, 65535)]
    public int Port { get; set; } = 9100;

    public string? Description { get; set; }

    public string Model { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    //[RegularExpression(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$", ErrorMessage = "Please enter a valid MAC Address")]
    public string MacAddress { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public PrinterManufacturers Manufacturer { get; set; }
    public PrinterLanguage Language { get; set; }
    public PrinterStatus Status { get; set; }
    public WorkflowType WorkflowType { get; set; } = WorkflowType.Printer;
}
