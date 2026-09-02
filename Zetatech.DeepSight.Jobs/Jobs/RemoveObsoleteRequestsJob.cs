using System;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Jobs.Abstractions;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Jobs;

public sealed class RemoveObsoleteRequestsJob : BasePeriodicJob
{
    private readonly IRequestsService _requestsService;

    public RemoveObsoleteRequestsJob(IRequestsService requestsService) : base(TimeSpan.FromHours(1), true)
    {
        _requestsService = requestsService ?? throw new ArgumentException("The provided requests service must be a valid instance", nameof(requestsService));
    }

    protected override async Task OnExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _requestsService.DeleteAsync(90, cancellationToken)
                              .ConfigureAwait(false);
    }
}
