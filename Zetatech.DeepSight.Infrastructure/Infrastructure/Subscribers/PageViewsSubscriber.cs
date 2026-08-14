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

public sealed class PageViewsSubscriber : BaseRabbitMQSubscriber<DeepSightDto>, IPageViewsSubscriber
{
    private readonly IPageViewsService _pageViewsService;

    public PageViewsSubscriber(IOptions<RabbitMQOptions> options,
                               IRabbitMQChannelFactory channelFactory,
                               IPageViewsService pageViewsService) : base(options, channelFactory)
    {
        _pageViewsService = pageViewsService ?? throw new ArgumentException("The provided service must be a valid instance", nameof(pageViewsService));
    }

    protected override async Task OnMessageReceivedAsync(RabbitMQMessage<DeepSightDto> message,
                                                         CancellationToken cancellationToken = default)
    {
        await _pageViewsService.CreateAsync(message.Body, cancellationToken)
                               .ConfigureAwait(false);
    }
}