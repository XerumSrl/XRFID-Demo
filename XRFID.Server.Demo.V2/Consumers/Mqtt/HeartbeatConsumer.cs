using MassTransit;
using Xerum.XFramework.Common.Enums;
using Xerum.XFramework.DBAccess.Uow;
using XRFID.Demo.Modules.Mqtt.Events;
using XRFID.Demo.Modules.Mqtt.Payloads;
using XRFID.Demo.Modules.Mqtt.Publishers;
using XRFID.Server.Demo.V2.Entities;
using XRFID.Server.Demo.V2.Entities.GPIOCfg;
using XRFID.Server.Demo.V2.Entities.ReaderCfg;
using XRFID.Server.Demo.V2.Repositories.Interfaces;

namespace XRFID.Server.Demo.V2.Consumers.Mqtt;

public class HeartbeatConsumer(IReaderRepository readerRepository,
                             IUnitOfWork uowk,
                             IZebraMqttCommandPublisher zebraCommandPublisher,
                             ILogger<HeartbeatConsumer> logger) : IConsumer<Heartbeat>
{

    public async Task Consume(ConsumeContext<Heartbeat> context)
    {
        logger.LogTrace("[Consume<Heartbeat>] message: {@message}", context.Message);

        try
        {
            var reader = (await readerRepository.GetAsync(q => q.Name == context.Message.HostName)).FirstOrDefault();

            if (reader is null)
            {

                //aggiungiamo in stato da approvare

                reader = await readerRepository.CreateAsync(new Reader
                {
                    Name = context.Message.HostName,
                    Reference = context.Message.HostName,
                    Status = ReaderStatus.Connected,
                    CorrelationId = Guid.Empty,
                    GPIOConfiguration = new GPIOConfiguration(),
                    Location = new Location
                    {
                        Name = context.Message.HostName + "_location",
                        Reference = context.Message.HostName + "_location"
                    },
                    ReaderConfiguration = new ReaderConfiguration
                    {
                        WorkflowSettings = new WorkflowSettings(),
                        ModeSpecificSettings = new ModeSpecificSettings()
                    }
                });

                await uowk.SaveAsync();

                //chiedo update di alcuni dati direttamente al reader, invocando un comando su ZebraMqtt.ZebraCommandConsumer
                Task version = zebraCommandPublisher.Publish(new RAWMQTTCommands
                {
                    Topic = "mcmds",
                    HostName = reader.Name,
                    Command = "get_version",
                    CommandId = Guid.NewGuid().ToString(),
                    Payload = new OneOfRAWMQTTCommandsPayload(),
                });

                //chiedo update di alcuni dati direttamente al reader, invocando un comando su ZebraMqtt.ZebraCommandConsumer
                Task network = zebraCommandPublisher.Publish(new RAWMQTTCommands
                {
                    Topic = "mcmds",
                    HostName = reader.Name,
                    Command = "get_network",
                    CommandId = Guid.NewGuid().ToString(),
                    Payload = new OneOfRAWMQTTCommandsPayload(),
                });

                await version;
                await network;

                return;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[Consume<SdkHeartbeat>] Unexpected exception");
            return;
        }
    }

}
