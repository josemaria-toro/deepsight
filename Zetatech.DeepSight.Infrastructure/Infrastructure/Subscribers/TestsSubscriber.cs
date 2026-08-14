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

public sealed class TestsSubscriber : BaseRabbitMQSubscriber<DeepSightDto>, ITestsSubscriber
{
    private readonly ITestsService _testsService;

    public TestsSubscriber(IOptions<RabbitMQOptions> options,
                           IRabbitMQChannelFactory channelFactory,
                           ITestsService testsService) : base(options, channelFactory)
    {
        _testsService = testsService ?? throw new ArgumentException("The provided service must be a valid instance", nameof(testsService));
    }

    protected override async Task OnMessageReceivedAsync(RabbitMQMessage<DeepSightDto> message,
                                                         CancellationToken cancellationToken = default)
    {
        await _testsService.CreateAsync(message.Body, cancellationToken)
                           .ConfigureAwait(false);
    }
}