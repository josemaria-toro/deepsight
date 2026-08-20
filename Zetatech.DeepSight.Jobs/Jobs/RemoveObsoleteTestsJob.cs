using System;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Jobs.Abstractions;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Jobs;

public sealed class RemoveObsoleteTestsJob : BaseTimerJob
{
    private readonly ITestsService _testsService;

    public RemoveObsoleteTestsJob(ITestsService testsService) : base(TimeSpan.FromHours(1), true)
    {
        _testsService = testsService ?? throw new ArgumentException("The provided tests service must be a valid instance", nameof(testsService));
    }

    protected override async Task OnExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _testsService.DeleteAsync(90, cancellationToken)
                           .ConfigureAwait(false);
    }
}
