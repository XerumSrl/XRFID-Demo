using MassTransit;
using Microsoft.Extensions.Logging;
using XRFID.Demo.Modules.Mqtt.Contracts;
using XRFID.Demo.Modules.Mqtt.Events;

namespace XRFID.Demo.Modules.Mqtt.Publishers;

public class ZebraMqttEventPublisher(IBusControl _busControl, ILogger<ZebraMqttEventPublisher> _logger) : IZebraMqttEventPublisher
{

    public async Task Publish(Heartbeat request)
    {
        // consumed by public class HeartbeatConsumer : IRequestConsumer<Heartbeat>
        await _busControl.Publish<Heartbeat>(request, ctx => { ctx.TimeToLive = TimeSpan.FromMinutes(5); });
    }

    public async Task Publish(ZebraGpiData request)
    {
        // consumed by public class ZebraGpioDataConsumer : IRequestConsumer<ZebraGpiData>
        await _busControl.Publish<ZebraGpiData>(request, ctx => { ctx.TimeToLive = TimeSpan.FromMinutes(5); });
    }

    public async Task Publish(ZebraTagData request)
    {
        // consumed by ZebraTagDataConsumer : IRequestConsumer<ZebraTagData>
        await _busControl.Publish<ZebraTagData>(request, ctx => { ctx.TimeToLive = TimeSpan.FromMinutes(5); });
    }

    public async Task Publish(ZebraDirectionalityTagData request)
    {
        await _busControl.Publish<ZebraDirectionalityTagData>(request, ctx => { ctx.TimeToLive = TimeSpan.FromMinutes(5); });
    }

    public async Task Publish(ZebraRawDirectionalityTagData request)
    {
        await _busControl.Publish<ZebraRawDirectionalityTagData>(request, ctx => { ctx.TimeToLive = TimeSpan.FromMinutes(5); });
    }
}
