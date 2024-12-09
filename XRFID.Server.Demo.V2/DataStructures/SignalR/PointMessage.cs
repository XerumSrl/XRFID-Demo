namespace XRFID.Server.Demo.V2.DataStructures.SignalR;

public class PointMessage : IEquatable<PointMessage?>
{
    public required double X { get; set; }
    public required double Y { get; set; }
    public required string EPC { get; set; }
    public required DateTimeOffset Timestamp { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PointMessage);
    }

    public bool Equals(PointMessage? other)
    {
        return other is not null &&
               X == other.X &&
               Y == other.Y &&
               EPC == other.EPC &&
               Timestamp.Equals(other.Timestamp);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, EPC, Timestamp);
    }

    public static bool operator ==(PointMessage? left, PointMessage? right)
    {
        return EqualityComparer<PointMessage>.Default.Equals(left, right);
    }

    public static bool operator !=(PointMessage? left, PointMessage? right)
    {
        return !(left == right);
    }
}
