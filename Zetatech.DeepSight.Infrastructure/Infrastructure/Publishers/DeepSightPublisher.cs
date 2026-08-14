using Microsoft.Extensions.Options;
using Zetatech.Accelerate.Messaging;
using Zetatech.Accelerate.Messaging.Abstractions;
using Zetatech.DeepSight.Domain.Publishers;

namespace Zetatech.DeepSight.Infrastructure.Publishers;

public sealed class DeepSightPublisher : BaseRabbitMQPublisher, IDeepSightPublisher
{
    public DeepSightPublisher(IOptions<RabbitMQOptions> options,
                              IRabbitMQChannelFactory channelFactory) : base(options, channelFactory)
    {
    }
}