using MassTransit;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Demo.Modules.Mqtt.Payloads;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Consumers.Mqtt;

public class MresponseConsumer(IReaderRepository readerRepository, IUnitOfWork uowk, ILogger<MresponseConsumer> logger) : IConsumer<GetVersionResponse>, IConsumer<GetNetworkResponse>, IConsumer<GetStatusResponse>, IConsumer<GetModeResponse>
{
    public async Task Consume(ConsumeContext<GetVersionResponse> context)
    {
        logger.LogTrace("[Consume<GetVersionResponse>] message: {@message}", context.Message);
        //var readerName = context.Message.HostName;

        var reader = (await readerRepository.GetAsync(q => q.Name == context.Message.HostName)).FirstOrDefault();
        if (reader is null)
        {
            logger.LogWarning("[Consume<GetVersionResponse>] No reader found in table for {readerName}", context.Message.HostName);
            return;
        }

        //update table reader information 
        reader.ReaderOS = context.Message.ReaderOS;
        reader.Model = context.Message.Model;
        reader.Version = context.Message.ReaderApplication;

        readerRepository.Update(ref reader);
        await uowk.SaveAsync();

    }

    public async Task Consume(ConsumeContext<GetNetworkResponse> context)
    {
        logger.LogTrace("[Consume<GetNetworkResponse>] message: {@message}", context.Message);

        await readerRepository.ExecuteUpdateAsync(
                    r => r.Name == context.Message.HostName,
                    a => a.SetProperty(p => p.Ip, context.Message.IpAddress)
                        .SetProperty(p => p.Uid, context.Message.MacAddress)
                        .SetProperty(p => p.MacAddress, context.Message.MacAddress)
                );
    }

    public async Task Consume(ConsumeContext<GetStatusResponse> context)
    {
        logger.LogTrace("[Consume<GetStatusResponse>] message: {@message}", context.Message);
        await Task.Delay(1);
    }

    public async Task Consume(ConsumeContext<GetModeResponse> context)
    {
        logger.LogTrace("[Consume<GetModeResponse>] message: {@message}", context.Message);

        await Task.Delay(1);
    }
}
