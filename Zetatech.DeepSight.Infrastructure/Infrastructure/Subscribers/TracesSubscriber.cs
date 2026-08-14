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

public sealed class TracesSubscriber : BaseRabbitMQSubscriber<DeepSightDto>, ITracesSubscriber
{
    private readonly ITracesService _tracesService;

    public TracesSubscriber(IOptions<RabbitMQOptions> options,
                            IRabbitMQChannelFactory channelFactory,
                            ITracesService tracesService) : base(options, channelFactory)
    {
        _tracesService = tracesService ?? throw new ArgumentException("The provided service must be a valid instance", nameof(tracesService));
    }

    protected override async Task OnMessageReceivedAsync(RabbitMQMessage<DeepSightDto> message,
                                                         CancellationToken cancellationToken = default)
    {
        await _tracesService.CreateAsync(message.Body, cancellationToken)
                            .ConfigureAwait(false);
    }
}