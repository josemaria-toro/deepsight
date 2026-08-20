using System;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Jobs.Abstractions;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Jobs;

public sealed class RemoveObsoleteTracesJob : BaseTimerJob
{
    private readonly ITracesService _tracesService;

    public RemoveObsoleteTracesJob(ITracesService tracesService) : base(TimeSpan.FromHours(1), true)
    {
        _tracesService = tracesService ?? throw new ArgumentException("The provided traces service must be a valid instance", nameof(tracesService));
    }

    protected override async Task OnExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _tracesService.DeleteAsync(90, cancellationToken)
                            .ConfigureAwait(false);
    }
}
