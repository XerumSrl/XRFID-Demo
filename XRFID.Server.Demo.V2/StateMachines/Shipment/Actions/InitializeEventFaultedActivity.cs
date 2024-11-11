using MassTransit;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.States;
using XRFID.Server.Demo.V2.Utilities;

namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Actions;

public class InitializeEventFaultedActivity :
    IStateMachineActivity<ShipmentState, Fault<IInitializeEvent>>
{
    private readonly CommandUtility commandUtility;
    private readonly IReaderRepository readerRepository;
    private readonly ILogger<InitializeEventFaultedActivity> logger;
    private readonly GpoUtility gpoUtility;

    public InitializeEventFaultedActivity(CommandUtility commandUtility,
                                          IReaderRepository readerRepository,
                                          ILogger<InitializeEventFaultedActivity> logger,
                                          GpoUtility gpoUtility)
    {
        this.commandUtility = commandUtility;
        this.readerRepository = readerRepository;
        this.logger = logger;
        this.gpoUtility = gpoUtility;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<ShipmentState, Fault<IInitializeEvent>> context, IBehavior<ShipmentState, Fault<IInitializeEvent>> next)
    {
        Reader reader = (await readerRepository.GetAsync(q => q.Id == context.Saga.ReaderId)).First();
        try
        {
            if (reader.Name is null)
            {
                throw new Exception($"Missing Reader Name for id {reader.Id}");
            }
            await commandUtility.StopReading(reader.Id, reader.Name);

            await gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutBuzzer?.Pin ?? 0, false); //Buzzer off
            await gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutGreenLED?.Pin ?? 0, true); //Green on
            await gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutYellowLED?.Pin ?? 0, true); //Yellow on
            await gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutRedLED?.Pin ?? 0, true); //Red on
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            throw new Exception(ex.Message);
        }

        ////reset reader correlation id
        //await readerManager.ResetCorrelationId(context.Message.Message.ReaderId);
        //logger.LogDebug("InitializeEventFaultedActivity|Resetting reader correlation id. ReaderId: {ReaderId}", context.Message.Message.ReaderId);

        await next.Execute(context);

    }

    public Task Faulted<TException>(BehaviorExceptionContext<ShipmentState, Fault<IInitializeEvent>, TException> context, IBehavior<ShipmentState, Fault<IInitializeEvent>> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("initializeEventFaulted");
    }
}