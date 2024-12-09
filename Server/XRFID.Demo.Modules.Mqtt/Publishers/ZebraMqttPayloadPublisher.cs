using MassTransit;
using XRFID.Demo.Modules.Mqtt.Payloads;

namespace XRFID.Demo.Modules.Mqtt.Publishers;


public class ZebraMqttPayloadPublisher(IBusControl _busControl) : IZebraMqttPayloadPublisher
{

    public async Task Publish(GetVersionResponse request)
    {
        await _busControl.Publish<GetVersionResponse>(request);
    }

    public async Task Publish(GetNetworkResponse request)
    {
        await _busControl.Publish<GetNetworkResponse>(request);
    }
}
