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
    Task<IList<TraceDto>> GetAsync(CancellationToken cancellationToken = default);
    Task<IList<TraceDto>> GetUsingFiltersAsync(String appName = null,
                                               IPAddress clientIpAddress = null,
                                               String hostname = null,
                                               Guid? tenant = null,
                                               DateTime? dateTimeFrom = null,
                                               DateTime? dateTimeTo = null,
                                               String category = null,
                                               String message = null,
                                               LogLevel? severity = null,
                                               CancellationToken cancellationToken = default);
}