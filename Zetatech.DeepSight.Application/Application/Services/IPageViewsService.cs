using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Zetatech.Accelerate.Application;
using Zetatech.DeepSight.Application.Dtos;

namespace Zetatech.DeepSight.Application.Services;

public interface IPageViewsService : IService
{
    Task<Guid> CreateAsync(PageViewDto pageViewDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(UInt32 daysToKeep, CancellationToken cancellationToken = default);
    Task<IList<PageViewDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Guid> PublishAsync(PageViewDto pageViewDto, CancellationToken cancellationToken = default);
    Task<IList<PageViewDto>> SearchAsync(String appName = null,
                                         IPAddress clientIpAddress = null,
                                         DateTime? dateTimeFrom = null,
                                         DateTime? dateTimeTo = null,
                                         String deviceType = null,
                                         String hostName = null,
                                         String name = null,
                                         String spanId = null,
                                         Guid? tenantId = null,
                                         String traceId = null,
                                         Uri url = null,
                                         String userAgent = null,
                                         CancellationToken cancellationToken = default);
}
