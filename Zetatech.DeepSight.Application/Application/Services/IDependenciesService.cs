using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Application;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface IDependenciesService : IService
{
    Task<Guid> CreateAsync(DependencyDto dependencyDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(UInt32 daysToKeep, CancellationToken cancellationToken = default);
    Task<IList<DependencyDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Guid> PublishAsync(DependencyDto dependencyDto, CancellationToken cancellationToken = default);
    Task<IList<DependencyDto>> SearchAsync(String appName = null,
                                           IPAddress clientIpAddress = null,
                                           DateTime? dateTimeFrom = null,
                                           DateTime? dateTimeTo = null,
                                           Double? durationFrom = null,
                                           Double? durationTo = null,
                                           String hostName = null,
                                           String name = null,
                                           String spanId = null,
                                           Boolean? success = null,
                                           String target = null,
                                           Guid? tenantId = null,
                                           String traceId = null,
                                           String type = null,
                                           CancellationToken cancellationToken = default);
}
