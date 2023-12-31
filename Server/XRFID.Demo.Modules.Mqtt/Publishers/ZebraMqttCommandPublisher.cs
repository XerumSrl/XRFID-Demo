﻿using MassTransit;
using Microsoft.Extensions.Logging;
using XRFID.Demo.Modules.Mqtt.Payloads;

namespace XRFID.Demo.Modules.Mqtt.Publishers;

public class ZebraMqttCommandPublisher : IZebraMqttCommandPublisher
{
    private readonly IBusControl busControl;
    private readonly ILogger<ZebraMqttCommandPublisher> logger;
    public ZebraMqttCommandPublisher(IBusControl busControl, ILogger<ZebraMqttCommandPublisher> logger)
    {
        this.busControl = busControl;
        this.logger = logger;
    }

    public async Task Publish(RAWMQTTCommands request)
    {
        logger.LogTrace(request.ToString());
        await busControl.Publish(request);
    }
}
