using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface IRequestsService : IDeepSightService
{
    Task<IList<RequestDto>> GetAsync(CancellationToken cancellationToken = default);
    Task<IList<RequestDto>> GetUsingFiltersAsync(String appName = null,
                                                 IPAddress clientIpAddress = null,
                                                 String hostname = null,
                                                 Guid? tenant = null,
                                                 DateTime? dateTimeFrom = null,
                                                 DateTime? dateTimeTo = null,
                                                 Double? durationFrom = null,
                                                 Double? durationTo = null,
                                                 String endpoint = null,
                                                 IPAddress ipAddress = null,
                                                 String name = null,
                                                 Int32? statusCode = null,
                                                 Boolean? success = null,
                                                 String type = null,
                                                 CancellationToken cancellationToken = default);
}
