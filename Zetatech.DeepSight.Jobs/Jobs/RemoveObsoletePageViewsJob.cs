using System;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Jobs.Abstractions;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Jobs;

public sealed class RemoveObsoletePageViewsJob : BaseTimerJob
{
    private readonly IPageViewsService _pageViewsService;

    public RemoveObsoletePageViewsJob(IPageViewsService pageViewsService) : base(TimeSpan.FromDays(1))
    {
        _pageViewsService = pageViewsService ?? throw new ArgumentException("The provided page views service must be a valid instance", nameof(pageViewsService));
    }

    protected override async Task OnExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _pageViewsService.DeleteAsync(90, cancellationToken)
                               .ConfigureAwait(false);
    }
}
