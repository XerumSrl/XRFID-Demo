using Xerum.XFramework.DBAccess.Entities;

namespace XRFID.Server.Demo.V2.Entities;

public class RawTagHistory : Entity
{
    public double Azimuth { get; set; }
    public int AzimuthConfidentiality { get; set; }
    public double AzimuthRadians { get; set; }
    public double Elevation { get; set; }
    public int ElevationConfidentiality { get; set; }
    public double ElevationRadians { get; set; }

    public double Distance { get; set; }
    public double DistanceCorrection { get; set; }
    public double DistanceRadians { get; set; }

    public double ResultX { get; set; }
    public double ResultY { get; set; }
    public double ResultZ { get; set; }

    public required string ReaderName { get; set; }
    public required string Epc { get; set; }
    public string? Format { get; set; }

    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.Now;

}
