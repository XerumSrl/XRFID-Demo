﻿using MassTransit;
using XRFID.Server.Demo.V2.StateMachines.Shipment.States;

namespace XRFID.Server.Demo.V2.StateMachines.Shipment.StateMachines;
public class ShipmentStateMachineDefinition : SagaDefinition<ShipmentState>
{
    private const int ConcurrencyLimit = 120; // this can go up, depending upon the database capacity

    public ShipmentStateMachineDefinition()
    {
        ConcurrentMessageLimit = 100;
        Endpoint(e =>
        {
            e.PrefetchCount = ConcurrencyLimit;
        });
    }

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<ShipmentState> sagaConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Interval(5, 1000));
        endpointConfigurator.UseInMemoryOutbox();
    }
}