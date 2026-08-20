using System;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Jobs.Abstractions;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Jobs;

public sealed class RemoveObsoleteErrorsJob : BaseTimerJob
{
    private readonly IErrorsService _errorsService;

    public RemoveObsoleteErrorsJob(IErrorsService errorsService) : base(TimeSpan.FromHours(1), true)
    {
        _errorsService = errorsService ?? throw new ArgumentException("The provided errors service must be a valid instance", nameof(errorsService));
    }

    protected override async Task OnExecuteAsync(CancellationToken cancellationToken = default)
    {
        await _errorsService.DeleteAsync(90, cancellationToken)
                            .ConfigureAwait(false);
    }
}
