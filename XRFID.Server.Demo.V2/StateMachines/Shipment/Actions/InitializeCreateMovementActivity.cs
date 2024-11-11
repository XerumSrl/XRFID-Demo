using MassTransit;
using Xerum.XFramework.Common.Enums;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Demo.Common.Enumerations;
using XRFID.Server.Demo.V2.Contracts;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.States;

namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Actions;
internal class InitializeCreateMovementActivity(IReaderRepository _readerRepository,
                                            IMovementRepository _movementRepository,
                                            ILoadingUnitRepository _loadingUnitRepository,
                                            ILoadingUnitItemRepository _loadingUnitItemRepository,
                                            IUnitOfWork _uowk,
                                            ILogger<InitializeCreateMovementActivity> _logger) : IStateMachineActivity<ShipmentState, IGpiEvent>
{

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<ShipmentState, IGpiEvent> context, IBehavior<ShipmentState, IGpiEvent> next)
    {
        Reader? reader = (await _readerRepository.GetTrackedAsync(q => q.Id == context.Saga.ReaderId)).FirstOrDefault();
        if (reader is null || context.Saga.ReaderId == Guid.Empty)
        {
            _logger.LogWarning("CreateMovementActivity|Unable to initialize shipment state machine. Reader id is empty");
            throw new InvalidOperationException("CreateMovementActivity|Reader id is empty");
        }

        //Creo la movement dalla loading unit
        LoadingUnit? loadingUnit = (await _loadingUnitRepository.GetAsync(q => q.ReaderId == reader.Id && q.IsActive))
                                                      .OrderByDescending(o => o.DateCreated)
                                                      .FirstOrDefault();
        if (loadingUnit is null)
        {
            _logger.LogDebug("CreateMovementActivity|No LoadingUnitItems available");

            loadingUnit = await _loadingUnitRepository.CreateAsync(new LoadingUnit
            {
                Id = Guid.NewGuid(),
                Description = $"Shipment {DateTime.Now}",
                Name = $"{reader.Name}_{DateTime.Now}",
                Reference = $"{reader.Name}_{DateTime.Now}",
                ReaderId = context.Saga.ReaderId,
                Reader = reader,
                IsActive = true,
                IsConsolidated = false,
            });


            await _movementRepository.DeactivateByReaderId(reader.Id);
            if (context.Message.GpiId == 2)
            {

            }
            var newMovement = new Movement
            {
                ReaderId = reader.Id,
                Name = $"{reader.Name}_{DateTime.Now}",
                Reference = $"{reader.Reference}_{DateTime.Now}",
                IsActive = true,

                Direction = context.Message.GpiId == 2 ? MovementDirection.In :
                context.Message.GpiId == 3 ? MovementDirection.Out : MovementDirection.In,
            };
            await _movementRepository.CreateAsync(newMovement);
            await _uowk.SaveAsync();
        }
        else
        {
            List<LoadingUnitItem> loadingUnitItems = await _loadingUnitItemRepository.GetAsync(q => q.LoadingUnitId == loadingUnit.Id);
            if (loadingUnitItems is null)
            {
                _logger.LogDebug("CreateMovementActivity|No LoadingUnitItems available");
            }


            await _movementRepository.DeactivateByReaderId(reader.Id);

            var newMovement = new Movement
            {
                ReaderId = reader.Id,
                Name = $"{reader.Name}_{DateTime.Now}",
                Reference = $"{reader.Reference}_{DateTime.Now}",
                IsActive = true,

                Direction = context.Message.GpiId == 2 ? MovementDirection.In :
                context.Message.GpiId == 3 ? MovementDirection.Out : MovementDirection.In,

                MovementItems = new List<MovementItem>(),
            };

            if (loadingUnitItems is not null)
            {
                foreach (var loadingUnitItem in loadingUnitItems)
                {
                    if (loadingUnitItem.Status == ItemStatus.NotFound ||
                        loadingUnitItem.Status == ItemStatus.Found)
                    {
                        MovementItem movementItem = new MovementItem
                        {
                            Name = loadingUnitItem.Name,
                            Reference = $"{loadingUnitItem.Reference}_{DateTime.Now}",

                            Description = loadingUnitItem.Description ?? loadingUnitItem.Epc,
                            SerialNumber = loadingUnitItem.SerialNumber ?? string.Empty,
                            Epc = loadingUnitItem.Epc,

                            ZoneName = reader.Location?.Name ?? string.Empty,

                            LoadingUnitItemId = loadingUnitItem.Id,

                            MovementId = newMovement.Id,
                            PreviousZoneName = string.Empty,

                        };

                        newMovement.MovementItems.Add(movementItem);
                    }

                }
            }
            await _movementRepository.CreateAsync(newMovement);
            await _uowk.SaveAsync();
        }

        var movement = (await _movementRepository.GetAsync(q => q.ReaderId == reader.Id && q.IsActive)).FirstOrDefault();
        if (movement is null)
        {
            _logger.LogWarning("CreateMovementActivity|Missing movement for reader {Id}", reader.Id);
            throw new InvalidOperationException($"CreateMovementActivity|Missing movement for reader {reader.Id}");
        }

        // faccio coincidere le due informazioni
        context.Saga.MovementId = movement.Id;

        //Send message to refresh UI
        await context.Publish(new StateMachineUiTagPublish
        {
            ReaderId = context.Message.ReaderId,
            ActivMoveId = movement.Id,
        });

        //@@TODO not make sense
        #region Update Reader
        if (movement.Id != context.Saga.MovementId)
        {
            _logger.LogDebug("StartActivity|Updating reader {Id}: CorrelationId: {CorrelationId} -> {CorrelationId} MovementId: {MovementId}",
                reader.Id, reader.CorrelationId, context.Saga.CorrelationId, context.Saga.MovementId);

            //reader.ActiveMovementId = context.Saga.MovementId;

            _readerRepository.Update(ref reader);
            await _uowk.SaveAsync();
        }
        #endregion

        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<ShipmentState, IGpiEvent, TException> context, IBehavior<ShipmentState, IGpiEvent> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("createMovement");
    }
}
