using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Application;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface IRequestsService : IService
{
    Task<Guid> CreateAsync(RequestDto requestDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(UInt32 daysToKeep, CancellationToken cancellationToken = default);
    Task<IList<RequestDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Guid> PublishAsync(RequestDto requestDto, CancellationToken cancellationToken = default);
    Task<IList<RequestDto>> SearchAsync(String appName = null,
                                        IPAddress clientIpAddress = null,
                                        DateTime? dateTimeFrom = null,
                                        DateTime? dateTimeTo = null,
                                        Double? durationFrom = null,
                                        Double? durationTo = null,
                                        String endpoint = null,
                                        String hostName = null,
                                        IPAddress ipAddress = null,
                                        String name = null,
                                        String spanId = null,
                                        Int32? statusCode = null,
                                        Boolean? success = null,
                                        Guid? tenantId = null,
                                        String traceId = null,
                                        String type = null,
                                        CancellationToken cancellationToken = default);
}
