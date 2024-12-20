﻿using MassTransit;

namespace XRFID.Server.Demo.V2.StateMachines.Shipment.Consumers;

internal class ShipmentGpioConsumerDefinition :
    ConsumerDefinition<ShipmentGpioConsumer>
{

    public ShipmentGpioConsumerDefinition()
    {
        ConcurrentMessageLimit = 100;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                                              IConsumerConfigurator<ShipmentGpioConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Interval(5, 1000));
        endpointConfigurator.UseInMemoryOutbox();
    }

}
