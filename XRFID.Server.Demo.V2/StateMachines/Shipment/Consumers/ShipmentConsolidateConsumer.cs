using MassTransit;
using XRFID.Server.Demo.V2.Repositories.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.Contracts;

namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Consumers;
public class ShipmentConsolidateConsumer(IMovementRepository movementRepository,
                                        ILogger<ShipmentConsolidateConsumer> logger) : IConsumer<ShipmentMovementConsolidateRequest>
{

    public async Task Consume(ConsumeContext<ShipmentMovementConsolidateRequest> context)
    {
        try
        {
            //aggiorno lo status dei movementItems sul db
            var movement = (await movementRepository.GetAsync(q => q.ReaderId == context.Message.ReaderId && q.IsActive)).FirstOrDefault();
            if (movement is null)
            {
                logger.LogWarning("ShipmentConsolidateConsumer|No active movement for {ReaderId}", context.Message.ReaderId);
                return;
            }

            logger.LogDebug("ShipmentMovementConsolidateRequest|Consolidated movement {MovementId}", context.Message.MovementId);

            //TODO reimplement update tag status
            //var updateMovement = await movementRepository.UpdateStatusAsync(context.Message.MovementId);
            logger.LogDebug("ShipmentMovementConsolidateRequest|Consolidated movement {MovementId}", context.Message.MovementId);

        }
        catch (Exception ex)
        {
            logger.LogWarning("ShipmentConsolidateConsumer|{Message}", ex.Message);
        }

    }
}
