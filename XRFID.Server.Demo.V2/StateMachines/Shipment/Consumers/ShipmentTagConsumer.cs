using MassTransit;
using Xerum.XFramework.Common.Enums;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Demo.Common.Dto;
using XRFID.Server.Demo.V2.Contracts;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.Contracts;
using XRFID.Server.Demo.V2.Utilities;

namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Consumers;

public class ShipmentTagConsumer(IReaderRepository _readerRepository, IMovementRepository _movementRepository, IMovementItemRepository _movementItemRepository, IUnitOfWork _uowk, GpoUtility _gpoUtility, ILogger<ShipmentTagConsumer> _logger) : IConsumer<ShipmentTagData>
{

    public async Task Consume(ConsumeContext<ShipmentTagData> context)
    {
        _logger.LogDebug("[Consume<ShipmentTagData>] {CorrelationId}|ShipmentTagData begin consumer", context.Message.CorrelationId);
        _logger.LogDebug("[Consume<ShipmentTagData>] {CorrelationId}|ShipmentTagData working on ReaderId: {ReaderId} for Epc: {Epc}", context.Message.CorrelationId, context.Message.ReaderId, context.Message.TagAction.Epc);

        if (context.Message.ReaderId == Guid.Empty)
        {
            try
            {
                Reader? reader = (await _readerRepository.GetAsync(q => q.Name == context.Message.HostName)).FirstOrDefault();
                if (reader is null)
                {
                    throw new Exception($"Reader with name {context.Message.HostName} does not exist");
                }
                context.Message.ReaderId = reader.Id;

            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[Consume<ShipmentTagData>] ");
                return;
            }
        }

        try
        {
            _logger.LogDebug("[Consume<ShipmentTagData>] {CorrelationId}|Begin Consume for ReaderId {ReaderId} Epc: {Epc}", context.Message.CorrelationId, context.Message.ReaderId, context.Message.TagAction.Epc);

            var tagRequest = context.Message;
            _logger.LogTrace($"[Consume<ShipmentTagData>] {context.Message.CorrelationId}|TagShipmentRequest {tagRequest}");

            TagDto tag = tagRequest.TagAction;
            _logger.LogTrace($"[Consume<ShipmentTagData>] {context.Message.CorrelationId}|TagActionDto {tag}");

            if (tag is null)
            {
                _logger.LogDebug("[Consume<ShipmentTagData>] {CorrelationId}|No tag from context {@context}", context.Message.CorrelationId, context);
                return;
            }

            Movement? movement;
            var movementId = context.Message.MovementId;
            if (context.Message.MovementId == Guid.Empty)
            {
                movement = (await _movementRepository.GetAsync(q => q.ReaderId == context.Message.ReaderId && q.IsActive)).FirstOrDefault();
                if (movement is null)
                {
                    _logger.LogWarning("[Consume<ShipmentTagData>] {CorrelationId}|GetActiveMovement on ReaderId: {ReaderId} returned NULL", context.Message.CorrelationId, context.Message.ReaderId);
                    return;
                }
                movementId = movement.Id;
                _logger.LogTrace($"[Consume<ShipmentTagData>] {context.Message.CorrelationId}|getting movement info {@movement}", movement);
            }

            //logger.LogTrace("[Consume<ShipmentTagData>] {CorrelationId}|AddItemForShipment TagAction: {Action} tag: {@tag}", context.Message.CorrelationId, tag.Action, tag);

            #region Save MovementItem

            if (string.IsNullOrEmpty(tag.Epc))
            {
                throw new Exception("[Consume<ShipmentTagData>] Epc string is empty");
            }
            MovementItem? movementItem = (await _movementItemRepository.GetTrackedAsync(q => q.MovementId == movementId && q.Epc.ToUpper() == tag.Epc.ToUpper())).OrderByDescending(o => o.DateCreated).FirstOrDefault();

            if (movementItem is null || movementItem is not null && (movementItem.ZoneName != context.Message.TagAction.ZoneName || movementItem.PreviousZoneName != context.Message.TagAction.PrevZoneName))
            {
                _logger.LogTrace("[Consume<ShipmentTagData>] {Epc}|Begin adding Unexpected item", tag.Epc);

                var newItem = new MovementItem
                {
                    Name = tag.Epc.ToUpper(),
                    Reference = $"{tag.Epc.ToUpper()}_{DateTimeOffset.Now}",
                    Epc = tag.Epc.ToUpper(),
                    ZoneName = tag.ZoneName,
                    PreviousZoneName = tag.PrevZoneName,

                };
                newItem.Description = ItemStatus.Unexpected.ToString();

                newItem.MovementId = movementId;

                if (movementItem is null)
                    newItem.Status = ItemStatus.Unexpected;
                else
                    newItem.Status = tag.ItemStatus;

                newItem.ReadsCount = tag.TagSeenCount;
                newItem.FirstRead = DateTime.Now;
                newItem.LastRead = DateTime.Now;

                newItem.Rssi = tag.Rssi;
                newItem.Tid = tag.Tid;
                newItem.PC = tag.PC;
                newItem.Checked = tag.IsValid;
                newItem.IgnoreUntil = DateTime.Now;

                movementItem = await _movementItemRepository.CreateAsync(newItem);
                await _uowk.SaveAsync();

                _logger.LogDebug("[Consume<ShipmentTagData>] {Epc}|End adding Unexpected item: {@item}", tag.Epc, movementItem);

            }
            else if (movementItem is not null && movementItem.Status != ItemStatus.NotFound)
            {
                _logger.LogTrace("[Consume<ShipmentTagData>] {Epc}|Begin updating existing item: {@item}", tag.Epc, movementItem);

                movementItem.LastRead = DateTime.Now;
                movementItem.ReadsCount += tag.TagSeenCount;
                movementItem.Rssi = tag.Rssi;
                movementItem.Tid = tag.Tid;
                movementItem.PC = tag.PC;
                movementItem.Checked = tag.IsValid;

                movementItem = _movementItemRepository.Update(ref movementItem);
                await _uowk.SaveAsync();
            }
            else if (movementItem is not null && movementItem.Status == ItemStatus.NotFound)
            {
                _logger.LogTrace("[Consume<ShipmentTagData>] {Epc}|Begin updating not found item: {@item} to ItemStatus.Found", tag.Epc, movementItem);

                movementItem.FirstRead = DateTime.Now;
                movementItem.LastRead = DateTime.Now;
                movementItem.ReadsCount += tag.TagSeenCount;
                movementItem.Status = ItemStatus.Found;

                movementItem.Rssi = tag.Rssi;
                movementItem.Tid = tag.Tid;
                movementItem.PC = tag.PC;
                movementItem.Checked = tag.IsValid;

                _movementItemRepository.Update(ref movementItem);
                await _uowk.SaveAsync();

                _logger.LogDebug("[Consume<ShipmentTagData>] {Epc}|End updating not found item: {@item} to ItemStatus.Found", tag.Epc, movementItem);
            }

            #endregion

            if (movementItem is null)
            {
                _logger.LogWarning("[Consume<ShipmentTagData>] {CorrelationId}|AddItemForShipment result is null", context.Message.CorrelationId);
                return;
            }

            if (movementItem.Status == ItemStatus.Unexpected && movementItem.IgnoreUntil < DateTime.Now)
            {
                Reader? reader = (await _readerRepository.GetAsync(q => q.Id == context.Message.ReaderId)).FirstOrDefault();
                if (reader is not null && reader.Name is not null)
                {


                    await _gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutYellowLED?.Pin ?? 0, true); //Yellow on

                    //buzzer sound for 1 second
                    await _gpoUtility.SoundBuzzer(reader, 1000);
                }


                _logger.LogDebug("[Consume<ShipmentTagData>] {CorrelationId}|Unexpected item found for Epc: {Epc}", context.Message.CorrelationId, movementItem.Epc);
            }

            //Send message to update UI
            await context.Publish(new StateMachineUiTagPublish
            {
                ReaderId = context.Message.ReaderId,
                ActivMoveId = movementId,
                Tag = context.Message.TagAction,
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }
        finally
        {
            _logger.LogDebug("[Consume<ShipmentTagData>] {CorrelationId}|End Consume for ReaderId {ReaderId} Epc: {Epc}", context.Message.CorrelationId, context.Message.ReaderId, context.Message.TagAction.Epc);
        }


        _logger.LogDebug("[Consume<ShipmentTagData>] {CorrelationId}|ShipmentTagData end consumer", context.Message.CorrelationId);
    }

}
