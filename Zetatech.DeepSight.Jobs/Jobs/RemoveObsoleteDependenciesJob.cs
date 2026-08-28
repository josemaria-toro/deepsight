using System;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Jobs.Abstractions;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Jobs;

public sealed class RemoveObsoleteDependenciesJob : BasePeriodicJob
{
    private readonly IDependenciesService _dependenciesService;

    public RemoveObsoleteDependenciesJob(IDependenciesService dependenciesService) : base(TimeSpan.FromHours(1), true)
    {
        _dependenciesService = dependenciesService ?? throw new ArgumentException("The provided dependencies service must be a valid instance", nameof(dependenciesService));
    }

    protected override async Task OnExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _dependenciesService.DeleteAsync(90, cancellationToken)
                                  .ConfigureAwait(false);
    }
}
