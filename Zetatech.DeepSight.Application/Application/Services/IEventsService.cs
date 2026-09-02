using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface IEventsService : IDeepSightService
{
    Task<Guid> CreateAsync(EventDto eventDto, CancellationToken cancellationToken = default);
    Task<IList<EventDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Guid> PublishAsync(EventDto eventDto, CancellationToken cancellationToken = default);
    Task<IList<EventDto>> SearchAsync(String appName = null,
                                      IPAddress clientIpAddress = null,
                                      DateTime? dateTimeFrom = null,
                                      DateTime? dateTimeTo = null,
                                      String hostName = null,
                                      String name = null,
                                      String spanId = null,
                                      Guid? tenantId = null,
                                      String traceId = null,
                                      CancellationToken cancellationToken = default);
}
