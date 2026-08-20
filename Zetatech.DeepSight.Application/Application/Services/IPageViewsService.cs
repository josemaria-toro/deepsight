using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface IPageViewsService : IDeepSightService
{
    Task<IList<PageViewDto>> GetAsync(CancellationToken cancellationToken = default);
    Task<IList<PageViewDto>> GetUsingFiltersAsync(String appName = null,
                                                  IPAddress clientIpAddress = null,
                                                  String hostname = null,
                                                  Guid? tenant = null,
                                                  DateTime? dateTimeFrom = null,
                                                  DateTime? dateTimeTo = null,
                                                  String deviceType = null,
                                                  String name = null,
                                                  String spanId = null,
                                                  String traceId = null,
                                                  Uri url = null,
                                                  String userAgent = null,
                                                  CancellationToken cancellationToken = default);
}
