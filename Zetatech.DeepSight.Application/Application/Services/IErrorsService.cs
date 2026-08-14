using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface IErrorsService : IDeepSightService
{
    Task<IList<ErrorDto>> GetAsync(CancellationToken cancellationToken = default);
    Task<IList<ErrorDto>> GetUsingFiltersAsync(String appName = null,
                                               IPAddress clientIpAddress = null,
                                               String hostname = null,
                                               Guid? tenant = null,
                                               DateTime? dateTimeFrom = null,
                                               DateTime? dateTimeTo = null,
                                               String category = null,
                                               String message = null,
                                               LogLevel? severity = null,
                                               String type = null,
                                               CancellationToken cancellationToken = default);
}