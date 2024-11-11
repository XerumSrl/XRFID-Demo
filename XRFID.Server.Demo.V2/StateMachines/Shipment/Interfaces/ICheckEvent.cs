namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Interfaces;

public interface ICheckEvent
{
    Guid ReaderId { get; set; }

    Guid SessionId { get; }
    DateTime Timestamp { get; }

}
