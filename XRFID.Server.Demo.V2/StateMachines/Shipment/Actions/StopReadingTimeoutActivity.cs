using MassTransit;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.Schedule;
using XRFID.Server.Demo.V2.StateMachines.Shipment.States;
using XRFID.Server.Demo.V2.Utilities;

namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Actions;

public class StopReadingTimeoutActivity :
    IStateMachineActivity<ShipmentState, ReadingExpired>
{
    private readonly CommandUtility commandUtility;
    private readonly IReaderRepository readerRepository;
    private readonly ILogger<StopReadingTimeoutActivity> logger;
    private readonly GpoUtility gpoUtility;

    public StopReadingTimeoutActivity(CommandUtility commandUtility,
                                      IReaderRepository readerRepository,
                                      ILogger<StopReadingTimeoutActivity> logger,
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

    public async Task Execute(BehaviorContext<ShipmentState, ReadingExpired> context, IBehavior<ShipmentState, ReadingExpired> next)
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
            await gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutYellowLED?.Pin ?? 0, false); //Yellow off
            await gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutRedLED?.Pin ?? 0, false); //Red off

        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            throw new Exception(ex.Message);
        }

        await next.Execute(context);

    }

    public Task Faulted<TException>(BehaviorExceptionContext<ShipmentState, ReadingExpired, TException> context, IBehavior<ShipmentState, ReadingExpired> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("stopEvent");
    }
}