using MassTransit;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using System.Text.Json;
using Xerum.XFramework.MassTransit;
using XRFID.Demo.Modules.Mqtt.Interfaces;
using XRFID.Demo.Modules.Mqtt.Payloads;

namespace XRFID.Demo.Modules.Mqtt.Consumers;

public class ZebraCommandConsumer : IRequestConsumer<RAWMQTTCommands>
{
    private readonly IManagedMqttClientService mqttClientService;
    private readonly ILogger<ZebraCommandConsumer> logger;

    public ZebraCommandConsumer(IManagedMqttClientService mqttClientService, ILogger<ZebraCommandConsumer> logger)
    {
        this.mqttClientService = mqttClientService;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<RAWMQTTCommands> context)
    {
        try
        {
            logger.LogDebug($"ZebraCommandConsumer|RAWMQTTCommands consume begin. CommandId: {context.Message.CommandId}");

            logger.LogDebug($"ZebraCommandConsumer|Topic: {context.Message.Topic}");

            if (string.IsNullOrEmpty(context.Message.Topic) || string.IsNullOrEmpty(context.Message.HostName))
            {
                logger.LogError($"ZebraCommandConsumer|Missing context.Message required property: Topic: {context.Message.Topic} - HostName: {context.Message.HostName}");
                return;
            }

            //var payload = JsonConvert.DeserializeObject(JsonSerializer.Serialize(context.Message.Payload)); //https://stackoverflow.com/a/72200593
            var payload = JsonSerializer.Deserialize<dynamic>((JsonElement)context.Message.Payload); //new and improved method
            var command = JsonSerializer.Serialize(new RAWMQTTCommands
            {
                HostName = null,
                Topic = null,
                Command = context.Message.Command,
                CommandId = context.Message.CommandId,
                Payload = payload ?? new Object(),
            });
            logger.LogDebug(@"ZebraCommandConsumer|Command: {command}", command);

            //request update from reader
            var message = new ManagedMqttApplicationMessageBuilder()
                    .WithApplicationMessage(new MqttApplicationMessageBuilder()
                    .WithTopic($"{context.Message.Topic}/{context.Message.HostName}")
                    .WithPayload(command)
                    .Build())
                .Build();

            await mqttClientService.MqttClient.InternalClient.PublishAsync(message.ApplicationMessage);
            //await mqttClient.EnqueueAsync(message);

            logger.LogDebug($"ZebraCommandConsumer|RAWMQTTCommands consume end. CommandId: {context.Message.CommandId}");
        }
        catch (Exception ex)
        {
            logger.LogError($"Message: {ex.Message}\n{ex.InnerException}\n{ex.StackTrace}");
            throw;
        }


    }

}
