using XRFID.Demo.Common.Dto;
using XRFID.Demo.Modules.Mqtt.Payloads;

namespace XRFID.Demo.Modules.Mqtt.Publishers;

public interface IZebraMqttCommandPublisher
{
    Task Publish(RAWMQTTCommands request);
    Task SoundBuzzer(ReaderDto reader, RAWMQTTCommands command, uint onMilliseconds = 0);
}
