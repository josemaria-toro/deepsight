using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zetatech.DeepSight.Application.Builders;
using Zetatech.DeepSight.Application.Dtos;
using Zetatech.DeepSight.Application.Services;
using Zetatech.DeepSight.Domain.Publishers;
using Zetatech.DeepSight.Domain.Repositories;
using Zetatech.DeepSight.Infrastructure.Abstractions;

namespace Zetatech.DeepSight.Infrastructure.Services;

public sealed class PageViewsService : BaseDeepSightService, IPageViewsService
{
    private readonly IDeepSightPublisher _deepSightPublisher;
    private readonly IPageViewsRepository _pageViewsRepository;

    public PageViewsService(IDeepSightPublisher deepSightPublisher = null,
                            IPageViewsRepository pageViewsRepository = null)
    {
        _deepSightPublisher = deepSightPublisher;
        _pageViewsRepository = pageViewsRepository;
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_pageViewsRepository == null)
        {
            throw new NotSupportedException("The page views repository is not currently available");
        }

        var pageViewEntity = deepSightDto.ToPageViewEntity();

        await _pageViewsRepository.InsertAsync(pageViewEntity, cancellationToken)
                                  .ConfigureAwait(false);

        return pageViewEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        await _pageViewsRepository.DeleteAsync(x => x.Timestamp.Date < DateTime.UtcNow.Date.AddDays(-daysToKeep), cancellationToken)
                                  .ConfigureAwait(false);
    }
    public async Task<IList<PageViewDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await GetUsingFiltersAsync(cancellationToken: cancellationToken);
    }
    public async Task<IList<PageViewDto>> GetUsingFiltersAsync(String appName = null,
                                                               IPAddress clientIpAddress = null,
                                                               String hostname = null,
                                                               Guid? tenant = null,
                                                               DateTime? dateTimeFrom = null,
                                                               DateTime? dateTimeTo = null,
                                                               String deviceType = null,
                                                               String name = null,
                                                               Uri url = null,
                                                               String userAgent = null,
                                                               CancellationToken cancellationToken = default)
    {
        var queryable = await _pageViewsRepository.SelectAsync(cancellationToken: cancellationToken)
                                                  .ConfigureAwait(false);

        /* Standard Filters */

        if (tenant.HasValue)
        {
            queryable = queryable.Where(x => x.TenantId == tenant);
        }

        if (dateTimeFrom.HasValue)
        {
            queryable = queryable.Where(x => x.Timestamp >= dateTimeFrom);
        }

        if (dateTimeTo.HasValue)
        {
            queryable = queryable.Where(x => x.Timestamp <= dateTimeTo);
        }

        if (clientIpAddress != null)
        {
            queryable = queryable.Where(x => x.ClientIpAddress == clientIpAddress.ToString());
        }

        if (!String.IsNullOrEmpty(hostname))
        {
            queryable = queryable.Where(x => x.HostName == hostname);
        }

        if (!String.IsNullOrEmpty(appName))
        {
            queryable = queryable.Where(x => x.AppName == appName);
        }

        /* Custom Filters */

        if (!String.IsNullOrEmpty(deviceType))
        {
            queryable = queryable.Where(x => x.DeviceType == deviceType);
        }

        if (!String.IsNullOrEmpty(name))
        {
            queryable = queryable.Where(x => x.Name == name);
        }

        if (url != null)
        {
            queryable = queryable.Where(x => x.Url == url.ToString());
        }

        if (!String.IsNullOrEmpty(userAgent))
        {
            queryable = queryable.Where(x => x.AppName == appName);
        }

        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);

        return [.. listOfEntities.Select(x => x.ToPageViewDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_deepSightPublisher == null)
        {
            throw new NotSupportedException("The page views publisher is not currently available");
        }

        return await _deepSightPublisher.PublishAsync(deepSightDto, "pageviews", cancellationToken)
                                        .ConfigureAwait(false);
    }
}
