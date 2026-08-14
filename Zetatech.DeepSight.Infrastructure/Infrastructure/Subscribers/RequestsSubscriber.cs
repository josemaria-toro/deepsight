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

public sealed class RequestsSubscriber : BaseRabbitMQSubscriber<DeepSightDto>, IRequestsSubscriber
{
    private readonly IRequestsService _requestsService;

    public RequestsSubscriber(IOptions<RabbitMQOptions> options,
                              IRabbitMQChannelFactory channelFactory,
                              IRequestsService requestsService) : base(options, channelFactory)
    {
        _requestsService = requestsService ?? throw new ArgumentException("The provided service must be a valid instance", nameof(requestsService));
    }

    protected override async Task OnMessageReceivedAsync(RabbitMQMessage<DeepSightDto> message,
                                                         CancellationToken cancellationToken = default)
    {
        await _requestsService.CreateAsync(message.Body, cancellationToken)
                              .ConfigureAwait(false);
    }
}