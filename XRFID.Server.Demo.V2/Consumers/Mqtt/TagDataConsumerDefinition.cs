using MassTransit;

namespace XRFID.Server.Demo.V2.Consumers.Mqtt;

public class TagDataConsumerDefinition : ConsumerDefinition<TagDataConsumer>
{
    public TagDataConsumerDefinition()
    {
        // override the default endpoint name, for whatever reason
        //EndpointName = "ha-submit-order";

        // limit the number of messages consumed concurrently
        // this applies to the consumer only, not the endpoint
        ConcurrentMessageLimit = 100;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<TagDataConsumer> consumerConfigurator, IRegistrationContext context)
    {

        endpointConfigurator.UseMessageRetry(r => r.Interval(5, 1000));
        endpointConfigurator.UseInMemoryOutbox(context);
    }
}