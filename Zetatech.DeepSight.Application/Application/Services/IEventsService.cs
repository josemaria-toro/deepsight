using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface IEventsService : IDeepSightService
{
    Task<IList<EventDto>> GetAsync(CancellationToken cancellationToken = default);
    Task<IList<EventDto>> GetUsingFiltersAsync(String appName = null,
                                               IPAddress clientIpAddress = null,
                                               String hostname = null,
                                               Guid? tenant = null,
                                               DateTime? dateTimeFrom = null,
                                               DateTime? dateTimeTo = null,
                                               String name = null,
                                               CancellationToken cancellationToken = default);
}
