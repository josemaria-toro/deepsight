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

public sealed class ErrorsSubscriber : BaseRabbitMQSubscriber<DeepSightDto>, IErrorsSubscriber
{
    private readonly IErrorsService _errorsService;

    public ErrorsSubscriber(IOptions<RabbitMQOptions> options,
                            IRabbitMQChannelFactory channelFactory,
                            IErrorsService errorsService) : base(options, channelFactory)
    {
        _errorsService = errorsService ?? throw new ArgumentException("The provided service must be a valid instance", nameof(errorsService));
    }

    protected override async Task OnMessageReceivedAsync(RabbitMQMessage<DeepSightDto> message,
                                                         CancellationToken cancellationToken = default)
    {
        await _errorsService.CreateAsync(message.Body, cancellationToken)
                            .ConfigureAwait(false);
    }
}