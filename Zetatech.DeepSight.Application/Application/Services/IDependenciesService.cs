using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface IDependenciesService : IDeepSightService
{
    Task<IList<DependencyDto>> GetAsync(CancellationToken cancellationToken = default);
    Task<IList<DependencyDto>> GetUsingFiltersAsync(String appName = null,
                                                    IPAddress clientIpAddress = null,
                                                    String hostname = null,
                                                    Guid? tenant = null,
                                                    DateTime? dateTimeFrom = null,
                                                    DateTime? dateTimeTo = null,
                                                    Double? durationFrom = null,
                                                    Double? durationTo = null,
                                                    String name = null,
                                                    String spanId = null,
                                                    Boolean? success = null,
                                                    String target = null,
                                                    String traceId = null,
                                                    String type = null,
                                                    CancellationToken cancellationToken = default);
}
