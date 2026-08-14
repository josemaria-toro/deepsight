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

public sealed class DependenciesSubscriber : BaseRabbitMQSubscriber<DeepSightDto>, IDependenciesSubscriber
{
    private readonly IDependenciesService _dependenciesService;

    public DependenciesSubscriber(IOptions<RabbitMQOptions> options,
                                  IRabbitMQChannelFactory channelFactory,
                                  IDependenciesService dependenciesService) : base(options, channelFactory)
    {
        _dependenciesService = dependenciesService ?? throw new ArgumentException("The provided service must be a valid instance", nameof(dependenciesService));
    }
    protected override async Task OnMessageReceivedAsync(RabbitMQMessage<DeepSightDto> message,
                                                         CancellationToken cancellationToken = default)
    {
        await _dependenciesService.CreateAsync(message.Body, cancellationToken)
                                  .ConfigureAwait(false);
    }
}