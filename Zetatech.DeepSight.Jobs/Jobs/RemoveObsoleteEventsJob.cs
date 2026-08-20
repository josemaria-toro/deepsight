using System;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Jobs.Abstractions;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Jobs;

public sealed class RemoveObsoleteEventsJob : BaseTimerJob
{
    private readonly IEventsService _eventsService;

    public RemoveObsoleteEventsJob(IEventsService eventsService) : base(TimeSpan.FromHours(1), true)
    {
        _eventsService = eventsService ?? throw new ArgumentException("The provided events service must be a valid instance", nameof(eventsService));
    }

    protected override async Task OnExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _eventsService.DeleteAsync(90, cancellationToken)
                            .ConfigureAwait(false);
    }
}
