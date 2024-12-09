namespace XRFID.Server.Demo.V2.DataStructures.Misc;

public class PlotDataObject
{
    public class PlotDataObj : IEquatable<PlotDataObj?>
    {
        public string Color { get; set; } = $"#{Guid.NewGuid().ToString("N").Substring(0, 6)}";//Crea colore esadecimale casuale
        public required string Epc { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PlotDataObj);
        }

        public bool Equals(PlotDataObj? other)
        {
            return other is not null &&
                   Epc == other.Epc;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Epc);
        }

        public static bool operator ==(PlotDataObj? left, PlotDataObj? right)
        {
            return EqualityComparer<PlotDataObj>.Default.Equals(left, right);
        }

        public static bool operator !=(PlotDataObj? left, PlotDataObj? right)
        {
            return !(left == right);
        }
    }
}
