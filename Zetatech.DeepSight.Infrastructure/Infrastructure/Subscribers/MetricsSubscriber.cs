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

public sealed class MetricsSubscriber : BaseRabbitMQSubscriber<DeepSightDto>, IMetricsSubscriber
{
    private readonly IMetricsService _metricsService;

    public MetricsSubscriber(IOptions<RabbitMQOptions> options,
                             IRabbitMQChannelFactory channelFactory,
                             IMetricsService metricsService) : base(options, channelFactory)
    {
        _metricsService = metricsService ?? throw new ArgumentException("The provided service must be a valid instance", nameof(metricsService));
    }

    protected override async Task OnMessageReceivedAsync(RabbitMQMessage<DeepSightDto> message,
                                                         CancellationToken cancellationToken = default)
    {
        await _metricsService.CreateAsync(message.Body, cancellationToken)
                             .ConfigureAwait(false);
    }
}