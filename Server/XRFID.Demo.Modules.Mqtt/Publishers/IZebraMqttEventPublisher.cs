using XRFID.Demo.Modules.Mqtt.Contracts;
using XRFID.Demo.Modules.Mqtt.Events;

namespace XRFID.Demo.Modules.Mqtt.Publishers;

public interface IZebraMqttEventPublisher
{
    Task Publish(Heartbeat request);
    Task Publish(ZebraGpiData request);
    Task Publish(ZebraTagData request);
    Task Publish(ZebraDirectionalityTagData request);
    Task Publish(ZebraRawDirectionalityTagData request);
}
