using MassTransit;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using System.Text.Json;
using XRFID.Demo.Modules.Mqtt.Interfaces;
using XRFID.Demo.Modules.Mqtt.Payloads;

namespace XRFID.Demo.Modules.Mqtt.Consumers;

public class ZebraCommandConsumer(IManagedMqttClientService _mqttClientService, ILogger<ZebraCommandConsumer> _logger) : IConsumer<RAWMQTTCommands>
{

    public async Task Consume(ConsumeContext<RAWMQTTCommands> context)
    {
        try
        {
            _logger.LogDebug($"ZebraCommandConsumer|RAWMQTTCommands consume begin. CommandId: {context.Message.CommandId}");

            _logger.LogDebug($"ZebraCommandConsumer|Topic: {context.Message.Topic}");

            if (string.IsNullOrEmpty(context.Message.Topic) || string.IsNullOrEmpty(context.Message.HostName))
            {
                _logger.LogError($"ZebraCommandConsumer|Missing context.Message required property: Topic: {context.Message.Topic} - HostName: {context.Message.HostName}");
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
            _logger.LogDebug(@"ZebraCommandConsumer|Command: {command}", command);

            //request update from reader
            var message = new ManagedMqttApplicationMessageBuilder()
                    .WithApplicationMessage(new MqttApplicationMessageBuilder()
                    .WithTopic($"{context.Message.Topic}/{context.Message.HostName}")
                    .WithPayload(command)
                    .Build())
                .Build();

            await _mqttClientService.MqttClient.InternalClient.PublishAsync(message.ApplicationMessage);
            //await mqttClient.EnqueueAsync(message);

            _logger.LogDebug($"ZebraCommandConsumer|RAWMQTTCommands consume end. CommandId: {context.Message.CommandId}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Message: {ex.Message}\n{ex.InnerException}\n{ex.StackTrace}");
            throw;
        }


    }

}
