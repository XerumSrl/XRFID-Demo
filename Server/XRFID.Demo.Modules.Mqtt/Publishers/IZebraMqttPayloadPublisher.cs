using XRFID.Demo.Modules.Mqtt.Payloads;

namespace XRFID.Demo.Modules.Mqtt.Publishers;

public interface IZebraMqttPayloadPublisher
{
    Task Publish(GetVersionResponse request);
    Task Publish(GetNetworkResponse request);
}
