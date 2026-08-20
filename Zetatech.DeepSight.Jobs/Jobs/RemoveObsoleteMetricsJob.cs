using System;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Jobs.Abstractions;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Jobs;

public sealed class RemoveObsoleteMetricsJob : BaseTimerJob
{
    private readonly IMetricsService _metricsService;

    public RemoveObsoleteMetricsJob(IMetricsService metricsService) : base(TimeSpan.FromHours(1), true)
    {
        _metricsService = metricsService ?? throw new ArgumentException("The provided metrics service must be a valid instance", nameof(metricsService));
    }

    protected override async Task OnExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _metricsService.DeleteAsync(90, cancellationToken)
                             .ConfigureAwait(false);
    }
}
