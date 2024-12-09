namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Interfaces;

public interface IStatusEvent
{
    Guid SessionId { get; set; }
    string ReaderId { get; set; }
    string ListId { get; set; }
    DateTime Timestamp { get; set; }


    /// <summary>
    /// true -> Open
    /// false -> Completed
    /// </summary>
    bool Status { get; set; }
}
