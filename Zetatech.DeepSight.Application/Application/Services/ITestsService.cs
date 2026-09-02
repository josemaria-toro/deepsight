using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Application;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface ITestsService : IService
{
    Task<Guid> CreateAsync(TestDto testDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(UInt32 daysToKeep, CancellationToken cancellationToken = default);
    Task<IList<TestDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Guid> PublishAsync(TestDto testDto, CancellationToken cancellationToken = default);
    Task<IList<TestDto>> SearchAsync(String appName = null,
                                     IPAddress clientIpAddress = null,
                                     DateTime? dateTimeFrom = null,
                                     DateTime? dateTimeTo = null,
                                     Double? durationFrom = null,
                                     Double? durationTo = null,
                                     String hostName = null,
                                     String message = null,
                                     String name = null,
                                     String spanId = null,
                                     Boolean? success = null,
                                     Guid? tenantId = null,
                                     String traceId = null,
                                     CancellationToken cancellationToken = default);
}
