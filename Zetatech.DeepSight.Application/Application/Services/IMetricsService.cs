using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Application;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface IMetricsService : IService
{
    Task<Guid> CreateAsync(MetricDto metricDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(UInt32 daysToKeep, CancellationToken cancellationToken = default);
    Task<IList<MetricDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Guid> PublishAsync(MetricDto metricDto, CancellationToken cancellationToken = default);
    Task<IList<MetricDto>> SearchAsync(String appName = null,
                                       IPAddress clientIpAddress = null,
                                       DateTime? dateTimeFrom = null,
                                       DateTime? dateTimeTo = null,
                                       String dimension = null,
                                       String hostName = null,
                                       String name = null,
                                       String spanId = null,
                                       Guid? tenantId = null,
                                       String traceId = null,
                                       CancellationToken cancellationToken = default);
}
