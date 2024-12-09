namespace XRFID.Server.Demo.V2.Entities.ReaderCfg;

public class StopTriggerWithTimeout
{
    /// <summary>
    /// Number of Tags or Number of Inventory Attempts.
    /// </summary>
    public ushort Number { get; set; }

    /// <summary>
    /// Timeout for the operation.
    /// </summary>
    public uint Timeout { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is StopTriggerWithTimeout timeout &&
               Number == timeout.Number &&
               Timeout == timeout.Timeout;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Number, Timeout);
    }
}