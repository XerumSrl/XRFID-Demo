using MassTransit;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.States;
using XRFID.Server.Demo.V2.Utilities;

namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Actions;
public class InitializeActivity(ILogger<InitializeActivity> _logger,
                              IReaderRepository _readerRepository,
                              GpoUtility _gpoUtility,
                              IUnitOfWork _uowk) : IStateMachineActivity<ShipmentState, IInitializeEvent>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<ShipmentState, IInitializeEvent> context, IBehavior<ShipmentState, IInitializeEvent> next)
    {
        Reader reader = (await _readerRepository.GetTrackedAsync(q => q.Id == context.Saga.ReaderId)).First();

        if (reader is null || context.Message.ReaderId == Guid.Empty)
        {
            _logger.LogWarning("StartActivity|Unable to initialize shipment state machine. Reader id is empty");
            throw new InvalidOperationException("Unable to initialize shipment state machine. Reader id is empty");
        }

        if (reader.CorrelationId != context.Saga.CorrelationId)
        {
            _logger.LogDebug("StartActivity|Updating reader {Id}: CorrelationId: {CorrelationId} -> {CorrelationId} MovementId: {MovementId}",
                reader.Id, reader.CorrelationId, context.Saga.CorrelationId, context.Saga.MovementId);

            reader.CorrelationId = context.Saga.CorrelationId;

            _readerRepository.Update(ref reader);
            await _uowk.SaveAsync();
        }

        try
        {
            if (reader.Name is null)
            {
                throw new Exception($"Missing Reader Name for id {reader.Id}");
            }


            await _gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutBuzzer?.Pin ?? 0, false); //Buzzer off
            await _gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutGreenLED?.Pin ?? 0, true); //Green on
            await _gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutYellowLED?.Pin ?? 0, false); //Yellow off
            await _gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutRedLED?.Pin ?? 0, false); //Red off

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw new Exception(ex.Message);
        }

        await next.Execute(context);

    }

    public Task Faulted<TException>(BehaviorExceptionContext<ShipmentState, IInitializeEvent, TException> context, IBehavior<ShipmentState, IInitializeEvent> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("start");
    }
}
