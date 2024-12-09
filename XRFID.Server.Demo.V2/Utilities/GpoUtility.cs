using AutoMapper;
using XRFID.Demo.Common.Dto;
using XRFID.Demo.Modules.Mqtt.Payloads;
using XRFID.Demo.Modules.Mqtt.Publishers;
using XRFID.Server.Demo.V2.Entities;

namespace XRFID.Server.Demo.V2.Utilities;

public class GpoUtility(IZebraMqttCommandPublisher _zebraMqttCommandPublisher, IMapper _mapper, ILogger<GpoUtility> _logger)
{
    public async Task SetGpo(Guid readerId, string hostname, int gpoPort, bool gpoStatus)
    {
        try
        {

            await _zebraMqttCommandPublisher.Publish(new RAWMQTTCommands
            {
                ReaderId = readerId,
                Topic = "mcmds",
                HostName = hostname,
                Command = "set_gpo",
                CommandId = Guid.NewGuid().ToString(),
                Payload = new SetGpoCommand
                {
                    Port = gpoPort,
                    State = gpoStatus,
                }
            }

            );

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public async Task SoundBuzzer(Reader reader, uint onMilliseconds = 0)
    {
        RAWMQTTCommands commandOn = new RAWMQTTCommands
        {
            ReaderId = reader.Id,
            Topic = "mcmds",
            HostName = reader.Name,
            Command = "set_gpo",
            CommandId = Guid.NewGuid().ToString(),
            Payload = new SetGpoCommand
            {
                Port = reader.GPIOConfiguration?.GPOutBuzzer?.Pin ?? 0,
                State = reader.GPIOConfiguration?.GPOutBuzzer?.LogicOn ?? false
            }
        };

        ReaderDto readerDto = _mapper.Map<ReaderDto>(reader);

        await _zebraMqttCommandPublisher.SoundBuzzer(readerDto, commandOn);

        RAWMQTTCommands commandOff = new RAWMQTTCommands
        {
            ReaderId = reader.Id,
            Topic = "mcmds",
            HostName = reader.Name,
            Command = "set_gpo",
            CommandId = Guid.NewGuid().ToString(),
            Payload = new SetGpoCommand
            {
                Port = reader.GPIOConfiguration?.GPOutBuzzer?.Pin ?? 0,
                State = reader.GPIOConfiguration?.GPOutBuzzer?.LogicOff ?? false
            }
        };

        await _zebraMqttCommandPublisher.SoundBuzzer(readerDto, commandOff, onMilliseconds);
    }
}


public class GpoConfiguration
{
    public int GreenLed { get; set; } = 1;
    public int YellowLed { get; set; } = 3;
    public int RedLed { get; set; } = 2;
    public int Buzzer { get; set; } = 4;
}
