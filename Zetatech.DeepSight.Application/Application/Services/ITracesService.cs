using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface ITracesService : IDeepSightService
{
    Task<Guid> CreateAsync(TraceDto traceDto, CancellationToken cancellationToken = default);
    Task<IList<TraceDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Guid> PublishAsync(TraceDto traceDto, CancellationToken cancellationToken = default);
    Task<IList<TraceDto>> SearchAsync(String appName = null,
                                      String category = null,
                                      IPAddress clientIpAddress = null,
                                      DateTime? dateTimeFrom = null,
                                      DateTime? dateTimeTo = null,
                                      String hostName = null,
                                      String message = null,
                                      LogLevel? severity = null,
                                      String spanId = null,
                                      Guid? tenantId = null,
                                      String traceId = null,
                                      CancellationToken cancellationToken = default);
}
