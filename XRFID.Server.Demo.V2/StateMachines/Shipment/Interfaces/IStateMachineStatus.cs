namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Interfaces;

public interface IStateMachineStatus
{
    Guid ReaderId { get; }
    Guid SessionId { get; }
    string ItemValue { get; }

    string State { get; }
}
