using MassTransit;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.States;
using XRFID.Server.Demo.V2.Utilities;

namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Actions;

public class StopReadingActivity :
    IStateMachineActivity<ShipmentState, IGpiEvent>
{
    private readonly CommandUtility commandUtility;
    private readonly IReaderRepository readerRepository;
    private readonly IUnitOfWork uowk;
    private readonly ILogger<StopReadingActivity> logger;
    private readonly GpoUtility gpoUtility;

    public StopReadingActivity(CommandUtility commandUtility,
        IReaderRepository readerRepository,
        IUnitOfWork uowk,
        ILogger<StopReadingActivity> logger,
        GpoUtility gpoUtility)
    {
        this.commandUtility = commandUtility;
        this.readerRepository = readerRepository;
        this.uowk = uowk;
        this.logger = logger;
        this.gpoUtility = gpoUtility;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<ShipmentState, IGpiEvent> context, IBehavior<ShipmentState, IGpiEvent> next)
    {
        try
        {
            Reader reader = await readerRepository.GetAsync(context.Saga.ReaderId) ?? throw new Exception("Missing Reader");

            if (reader.Name is null)
            {
                throw new Exception($"Missing Reader Name for id {reader.Id}");
            }
            await commandUtility.StopReading(reader.Id, reader.Name);




            await gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutBuzzer?.Pin ?? 0, false); //Buzzer off
            await gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutGreenLED?.Pin ?? 0, false); //Green off
            await gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutYellowLED?.Pin ?? 0, false); //Yellow off
            await gpoUtility.SetGpo(reader.Id, reader.Name, reader.GPIOConfiguration.GPOutRedLED?.Pin ?? 0, true); //Red on

            await readerRepository.ExecuteUpdateAsync(r => r.Id == reader.Id, e => e.SetProperty(p => p.CorrelationId, Guid.Empty));

            logger.LogDebug("Reset CorrelationId");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            throw;
        }

        await next.Execute(context);

    }

    public Task Faulted<TException>(BehaviorExceptionContext<ShipmentState, IGpiEvent, TException> context, IBehavior<ShipmentState, IGpiEvent> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("gpiStopEvent");
    }
}