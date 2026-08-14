using System;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Application.Abstractions;
using Zetatech.DeepSight.Application.Dtos;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Infrastructure.Abstractions;

public abstract class BaseDeepSightService : BaseApplicationService, IDeepSightService
{
    public abstract Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                           CancellationToken cancellationToken = default);
    public abstract Task DeleteAsync(UInt32 daysToKeep,
                                     CancellationToken cancellationToken = default);
    public abstract Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                            CancellationToken cancellationToken = default);
}