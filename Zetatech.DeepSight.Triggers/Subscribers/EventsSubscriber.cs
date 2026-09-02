using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger _logger;

    public EventsSubscriber(IOptions<RabbitMQOptions> options,
                            IRabbitMQChannelFactory channelFactory,
                            IEventsService eventsService,
                            ILoggerFactory loggerFactory) : base(options, channelFactory)
    {
        _eventsService = eventsService ?? throw new ArgumentException("The provided service must be a valid instance", nameof(eventsService));
        _logger = loggerFactory.CreateLogger<EventsSubscriber>();
    }

    protected override async Task OnMessageReceivedAsync(RabbitMQMessage<DeepSightDto> message,
                                                         CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"Received a new event message with id {message.Id}");
        await _eventsService.CreateAsync(message.Body, cancellationToken)
                            .ConfigureAwait(false);
    }
}
