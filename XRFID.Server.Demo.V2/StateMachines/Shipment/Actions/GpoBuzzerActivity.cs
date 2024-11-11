using MassTransit;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.Schedule;
using XRFID.Server.Demo.V2.StateMachines.Shipment.States;
using XRFID.Server.Demo.V2.Utilities;

namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Actions;

public class GpoBuzzerActivity :
    IStateMachineActivity<ShipmentState, GpoBuzzerExpired>
{
    private readonly IReaderRepository readerRepository;
    private readonly ILogger<GpoBuzzerActivity> logger;
    private readonly GpoUtility gpoUtility;

    public GpoBuzzerActivity(IReaderRepository readerRepository,
                             ILogger<GpoBuzzerActivity> logger,
                             GpoUtility gpoUtility)
    {
        this.readerRepository = readerRepository;
        this.logger = logger;
        this.gpoUtility = gpoUtility;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<ShipmentState, GpoBuzzerExpired> context, IBehavior<ShipmentState, GpoBuzzerExpired> next)
    {
        logger.LogDebug("GpoBuzzerActivity|BuzzerGpoExpired on ReaderId: {ReaderId}", context.Message.CorrelationId);

        Reader reader = (await readerRepository.GetAsync(q => q.Id == context.Saga.ReaderId)).First();
        try
        {
            if (reader.Name is null)
            {
                throw new Exception($"Missing Reader Name for id {reader.Id}");
            }

            await gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutBuzzer?.Pin ?? 0, context.Message.GpoBuzzerValue); //Buzzer off
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            throw new Exception(ex.Message);
        }

        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<ShipmentState, GpoBuzzerExpired, TException> context, IBehavior<ShipmentState, GpoBuzzerExpired> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("gpoBuzzer");
    }
}