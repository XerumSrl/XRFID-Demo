using MassTransit;
using Microsoft.AspNetCore.SignalR;
using XRFID.Server.Demo.V2.Contracts;
using XRFID.Server.Demo.V2.Hubs;
using XRFID.Server.Demo.V2.Repositories.Interfaces;
using XRFID.Server.Demo.V2.Workers;

namespace XRFID.Server.Demo.V2.Consumers.Frontend;

public class UpdateUiConsumer(CheckPageWorker _worker,
                            IHubContext<UiMessageHub> _hubContext,
                            IReaderRepository _readerRepo,
                            ILogger<UpdateUiConsumer> _logger) : IConsumer<StateMachineUiTagPublish>, IConsumer<StateMachineStatusPublish>
{

    public async Task Consume(ConsumeContext<StateMachineUiTagPublish> context)
    {
        if (context.Message.Tag is null)
        {
            _logger.LogDebug("[Consume<StateMachineUiTagPublish>] State Machine Message Receved");
        }
        else
        {
            _logger.LogDebug("[Consume<StateMachineUiTagPublish>] Tag {Epc} Receved", context.Message.Tag.Epc);
        }

        //Reaload items
        if (await _worker.IdIsEqual(context.Message.ActivMoveId))
        {
            if (context.Message.Tag is not null)
            {
                await _worker.SetViewItem(context.Message.Tag.Epc);
            }
        }

        var reader = (await _readerRepo.GetAsync(r => r.Id == context.Message.ReaderId)).FirstOrDefault();

        if (reader is not null)
            //Send signalR messages
            await _hubContext.Clients.Groups(reader.Name).SendAsync("RefreshTag");
    }

    public async Task Consume(ConsumeContext<StateMachineStatusPublish> context)
    {
        var reader = (await _readerRepo.GetAsync(r => r.Id == context.Message.RederId)).FirstOrDefault();

        await _worker.EditSMStatus(context.Message.State);

        if (reader is not null)
            await _hubContext.Clients.Groups(reader.Name).SendAsync("RefreshState");
    }
}
