using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Zetatech.Accelerate.Application.Abstractions;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Infrastructure.Abstractions;

public abstract class BaseDeepSightService : BaseService, IDeepSightService
{
    private readonly ILogger _logger;

    protected BaseDeepSightService(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(GetType().Name);
    }

    public ILogger Logger => _logger;

    public abstract Task DeleteAsync(UInt32 daysToKeep, CancellationToken cancellationToken = default);
}