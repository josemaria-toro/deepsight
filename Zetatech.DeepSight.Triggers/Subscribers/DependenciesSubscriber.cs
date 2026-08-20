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

public sealed class DependenciesSubscriber : BaseRabbitMQSubscriber<DeepSightDto>, IDependenciesSubscriber
{
    private readonly IDependenciesService _dependenciesService;
    private readonly ILogger _logger;

    public DependenciesSubscriber(IOptions<RabbitMQOptions> options,
                                  IRabbitMQChannelFactory channelFactory,
                                  IDependenciesService dependenciesService,
                                  ILoggerFactory loggerFactory) : base(options, channelFactory)
    {
        _dependenciesService = dependenciesService ?? throw new ArgumentException("The provided service must be a valid instance", nameof(dependenciesService));
        _logger = loggerFactory.CreateLogger<DependenciesSubscriber>();
    }
    protected override async Task OnMessageReceivedAsync(RabbitMQMessage<DeepSightDto> message,
                                                         CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"Received a new dependency message with id {message.Id}");
        await _dependenciesService.CreateAsync(message.Body, cancellationToken)
                                  .ConfigureAwait(false);
    }
}
