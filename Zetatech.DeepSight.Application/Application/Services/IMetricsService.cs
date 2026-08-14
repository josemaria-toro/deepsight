using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface IMetricsService : IDeepSightService
{
    Task<IList<MetricDto>> GetAsync(CancellationToken cancellationToken = default);
    Task<IList<MetricDto>> GetUsingFiltersAsync(String appName = null,
                                                IPAddress clientIpAddress = null,
                                                String hostname = null,
                                                Guid? tenant = null,
                                                DateTime? dateTimeFrom = null,
                                                DateTime? dateTimeTo = null,
                                                String dimension = null,
                                                String name = null,
                                                CancellationToken cancellationToken = default);
}