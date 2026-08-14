using System;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Application;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface IDeepSightService : IApplicationService
{
    Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                           CancellationToken cancellationToken = default);
    Task DeleteAsync(UInt32 daysToKeep,
                     CancellationToken cancellationToken = default);
    Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                            CancellationToken cancellationToken = default);
}