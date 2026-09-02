using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Zetatech.Accelerate.Application.Abstractions;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Infrastructure.Abstractions;

public abstract class BaseDeepSightService : BaseService
{
    private readonly ILogger _logger;

    protected BaseDeepSightService(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(GetType().Name);
    }

    public ILogger Logger => _logger;
}
