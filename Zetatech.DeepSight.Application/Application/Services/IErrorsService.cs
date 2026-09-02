using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Zetatech.Accelerate.Application;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface IErrorsService : IService
{
    Task<Guid> CreateAsync(ErrorDto errorDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(UInt32 daysToKeep, CancellationToken cancellationToken = default);
    Task<IList<ErrorDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Guid> PublishAsync(ErrorDto errorDto, CancellationToken cancellationToken = default);
    Task<IList<ErrorDto>> SearchAsync(String appName = null,
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
                                      String type = null,
                                      CancellationToken cancellationToken = default);
}
