namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Contracts;
public record ShipmentMovementConsolidateRequest
{
    public Guid ReaderId { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid MovementId { get; set; }
}
