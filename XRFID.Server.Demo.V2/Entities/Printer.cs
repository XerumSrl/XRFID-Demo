using Xerum.XFramework.Common.Enums;
using Xerum.XFramework.DBAccess.Entities;

namespace XRFID.Server.Demo.V2.Entities;

public class Printer : AuditEntity
{
    public string? Description { get; set; }

    public string? Uid { get; set; }

    public string? Model { get; set; }

    public string? Version { get; set; }

    public LicenseStatus LicenseStatus { get; set; }

    public string Ip { get; set; } = "127.0.0.1";

    public int Port { get; set; } = 9100;

    public string? MacAddress { get; set; }

    public string? SerialNumber { get; set; }

    public PrinterManufacturers Manufacturer { get; set; }

    public PrinterLanguage Language { get; set; }

    public PrinterStatus Status { get; set; }

    #region EF Logic Links

    public ICollection<Movement> Movements { get; set; } = [];

    #endregion
}
