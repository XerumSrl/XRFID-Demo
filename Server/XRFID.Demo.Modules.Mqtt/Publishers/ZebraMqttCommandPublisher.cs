using MassTransit;
using Microsoft.Extensions.Logging;
using XRFID.Demo.Common.Dto;
using XRFID.Demo.Modules.Mqtt.Payloads;

namespace XRFID.Demo.Modules.Mqtt.Publishers;

public class ZebraMqttCommandPublisher(IBusControl _busControl, ILogger<ZebraMqttCommandPublisher> _logger) : IZebraMqttCommandPublisher
{

    public async Task Publish(RAWMQTTCommands request)
    {
        _logger.LogTrace(request.ToString());
        await _busControl.Publish(request);
    }

    public async Task SoundBuzzer(ReaderDto reader, RAWMQTTCommands command, uint onMilliseconds = 0)
    {
        TimeSpan onTime;

        if (onMilliseconds == 0)
        {
            if (reader.GPIOConfiguration?.GPOutBuzzer?.Duration is null || reader.GPIOConfiguration.GPOutBuzzer.Duration < 0)
            {
                onTime = TimeSpan.FromMilliseconds(500);
            }
            else
            {
                onTime = TimeSpan.FromMilliseconds(reader.GPIOConfiguration.GPOutBuzzer.Duration);
            }
        }
        else
        {
            onTime = TimeSpan.FromMilliseconds(onMilliseconds);
        }

        await _busControl.Publish(command, ctx => ctx.Delay = onTime);


    }
}
