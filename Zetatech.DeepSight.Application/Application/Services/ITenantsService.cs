using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Application;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface ITenantsService : IService
{
    Task<Guid> CreateAsync(TenantDto tenantDto, CancellationToken cancellationToken = default);
    Task DisableAsync(Guid id, CancellationToken cancellationToken = default);
    Task EnableAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IList<TenantDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IList<TenantDto>> SearchAsync(Boolean? enabled = null,
                                       Guid? id = null,
                                       String name = null,
                                       CancellationToken cancellationToken = default);
}
