using Xerum.XFramework.Common.Enums;
using Xerum.XFramework.DBAccess.Entities;
using XRFID.Server.Demo.V2.Entities.GPIOCfg;
using XRFID.Server.Demo.V2.Entities.ReaderCfg;

namespace XRFID.Server.Demo.V2.Entities;

public class Reader : AuditEntity
{

    public string? Description { get; set; }

    public string? Uid { get; set; }

    public string? Model { get; set; }

    public string? Version { get; set; }

    public LicenseStatus LicenseStatus { get; set; }

    public string Ip { get; set; } = "127.0.0.1";

    public string? MacAddress { get; set; }

    public string? ReaderOS { get; set; }

    public RfidManufacturers Manufacturer { get; set; } = RfidManufacturers.Zebra;

    public ReaderStatus Status { get; set; } = ReaderStatus.Disabled;

    public WorkflowType WorkflowType { get; set; } = WorkflowType.Inventory;

    public required GPIOConfiguration GPIOConfiguration { get; set; }

    public required ReaderConfiguration ReaderConfiguration { get; set; }

    public byte[]? Antennas { get; set; }
    public Guid? LocationId { get; set; }

    public Guid CorrelationId { get; set; } = Guid.Empty;

    public DateTimeOffset HeartbeatTime { get; set; } = DateTimeOffset.MinValue;

    public string? ReaderMode { get; set; }

    public bool LocationingEnabled { get; set; }

    #region EF Logic Links

    //public string? LocationName { get; set; } = "location";
    public Location? Location { get; set; }

    public ICollection<Movement> Movements { get; set; } = [];

    public ICollection<LoadingUnit> LoadingUnits { get; set; } = [];

    #endregion
}
