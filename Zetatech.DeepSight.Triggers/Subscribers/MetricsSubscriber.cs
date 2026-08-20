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

public sealed class MetricsSubscriber : BaseRabbitMQSubscriber<DeepSightDto>, IMetricsSubscriber
{
    private readonly ILogger _logger;
    private readonly IMetricsService _metricsService;

    public MetricsSubscriber(IOptions<RabbitMQOptions> options,
                             IRabbitMQChannelFactory channelFactory,
                             IMetricsService metricsService,
                             ILoggerFactory loggerFactory) : base(options, channelFactory)
    {
        _logger = loggerFactory.CreateLogger<MetricsSubscriber>();
        _metricsService = metricsService ?? throw new ArgumentException("The provided service must be a valid instance", nameof(metricsService));
    }

    protected override async Task OnMessageReceivedAsync(RabbitMQMessage<DeepSightDto> message,
                                                         CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"Received a new metric message with id {message.Id}");
        await _metricsService.CreateAsync(message.Body, cancellationToken)
                             .ConfigureAwait(false);
    }
}
