using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Zetatech.Accelerate.Messaging;
using Zetatech.Accelerate.Messaging.Abstractions;
using Zetatech.Accelerate.Messaging.Messages;
using Zetatech.DeepSight.Application.Dtos;
using Zetatech.DeepSight.Application.Services;
using Zetatech.DeepSight.Application.Subscribers;

namespace Zetatech.DeepSight.Infrastructure.Subscribers;

public sealed class EventsSubscriber : BaseRabbitMQSubscriber<DeepSightDto>, IEventsSubscriber
{
    private readonly IEventsService _eventsService;

    public EventsSubscriber(IOptions<RabbitMQOptions> options,
                            IRabbitMQChannelFactory channelFactory,
                            IEventsService eventsService) : base(options, channelFactory)
    {
        _eventsService = eventsService ?? throw new ArgumentException("The provided service must be a valid instance", nameof(eventsService));
    }

    protected override async Task OnMessageReceivedAsync(RabbitMQMessage<DeepSightDto> message,
                                                         CancellationToken cancellationToken = default)
    {
        await _eventsService.CreateAsync(message.Body, cancellationToken)
                            .ConfigureAwait(false);
    }
}