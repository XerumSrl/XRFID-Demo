using System.Text.Json.Serialization;
using Xerum.XFramework.Common.Dto;
using Xerum.XFramework.Common.Enums;
using XRFID.Demo.Common.Dto.GPIOConfiguration;
using XRFID.Demo.Common.Dto.ReaderConfiguration;

namespace XRFID.Demo.Common.Dto;

public class ReaderDto : BaseDto
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

    public WorkflowType WorkflowType { get; set; }

    public required GPIOConfigurationDto GPIOConfiguration { get; set; }

    public required ReaderConfigurationDto ReaderConfiguration { get; set; }

    public byte[]? Antennas { get; set; }

    [Obsolete]
    public Guid CorrelationId { get; set; } = Guid.Empty;

    [JsonIgnore]
    public ushort[] AntennaArray
    {
        get
        {
            if (Antennas is null)
            {
                return [];
            }
            ushort[] result = new ushort[Antennas.Length / 2];
            Buffer.BlockCopy(Antennas, 0, result, 0, Antennas.Length);
            return result;
        }

        set
        {
            Antennas = new byte[value.Length * 2];
            Buffer.BlockCopy(value, 0, Antennas, 0, value.Length * 2);
        }
    }

    public DateTimeOffset HeartbeatTime { get; set; }

    public bool LocationingEnabled { get; set; }


    #region Data Relations

    public ICollection<MovementDto> Movements { get; set; } = [];

    public ICollection<LoadingUnitDto> LoadingUnits { get; set; } = [];

    #endregion
}
