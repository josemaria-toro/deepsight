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

public sealed class PageViewsSubscriber : BaseRabbitMQSubscriber<DeepSightDto>, IPageViewsSubscriber
{
    private readonly ILogger _logger;
    private readonly IPageViewsService _pageViewsService;

    public PageViewsSubscriber(IOptions<RabbitMQOptions> options,
                               IRabbitMQChannelFactory channelFactory,
                               IPageViewsService pageViewsService,
                               ILoggerFactory loggerFactory) : base(options, channelFactory)
    {
        _logger = loggerFactory.CreateLogger<PageViewsSubscriber>();
        _pageViewsService = pageViewsService ?? throw new ArgumentException("The provided service must be a valid instance", nameof(pageViewsService));
    }

    protected override async Task OnMessageReceivedAsync(RabbitMQMessage<DeepSightDto> message,
                                                         CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"Received a new page view message with id {message.Id}");
        await _pageViewsService.CreateAsync(message.Body, cancellationToken)
                               .ConfigureAwait(false);
    }
}
