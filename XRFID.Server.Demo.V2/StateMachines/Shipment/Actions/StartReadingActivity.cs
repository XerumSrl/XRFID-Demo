using MassTransit;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Repositories.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.Interfaces;
using XRFID.Server.Demo.V2.StateMachines.Shipment.States;
using XRFID.Server.Demo.V2.Utilities;

namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Actions;

public class StartReadingActivity :
    IStateMachineActivity<ShipmentState, IGpiEvent>
{
    private readonly CommandUtility commandUtility;
    private readonly IReaderRepository readerRepository;
    private readonly ILogger<StartReadingActivity> logger;

    public StartReadingActivity(CommandUtility commandUtility,
                                IReaderRepository readerRepository,
                                ILogger<StartReadingActivity> logger)
    {
        this.commandUtility = commandUtility;
        this.readerRepository = readerRepository;
        this.logger = logger;
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<ShipmentState, IGpiEvent> context, IBehavior<ShipmentState, IGpiEvent> next)
    {
        Reader reader = (await readerRepository.GetAsync(q => q.Id == context.Saga.ReaderId)).First();

        try
        {
            if (reader.Name is null)
            {
                throw new Exception($"Missing Reader Name for id {reader.Id}");
            }
            await commandUtility.StartReading(reader.Id, reader.Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            throw new Exception(ex.Message);
        }

        await next.Execute(context);

    }

    public Task Faulted<TException>(BehaviorExceptionContext<ShipmentState, IGpiEvent, TException> context, IBehavior<ShipmentState, IGpiEvent> next) where TException : Exception
    {
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("gpiStartEvent");
    }
}