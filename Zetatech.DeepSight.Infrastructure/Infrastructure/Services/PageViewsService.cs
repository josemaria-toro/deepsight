using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zetatech.DeepSight.Application.Builders;
using Zetatech.DeepSight.Application.Dtos;
using Zetatech.DeepSight.Application.Services;
using Zetatech.DeepSight.Domain.Publishers;
using Zetatech.DeepSight.Domain.Repositories;
using Zetatech.DeepSight.Infrastructure.Abstractions;

namespace Zetatech.DeepSight.Infrastructure.Services;

public sealed class PageViewsService : BaseDeepSightService, IPageViewsService
{
    private readonly ILogger _logger;
    private readonly IDeepSightPublisher _pageViewsPublisher;
    private readonly IPageViewsRepository _pageViewsRepository;

    public PageViewsService(ILoggerFactory loggerFactory,
                            IDeepSightPublisher pageViewsPublisher = null,
                            IPageViewsRepository pageViewsRepository = null)
    {
        _logger = loggerFactory.CreateLogger<PageViewsService>();
        _pageViewsPublisher = pageViewsPublisher;
        _pageViewsRepository = pageViewsRepository;
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_pageViewsRepository == null)
        {
            throw new NotSupportedException("The page views repository is not currently available");
        }

        _logger.LogDebug("Building page view entity from dto");
        var pageViewEntity = deepSightDto.ToPageViewEntity();
        _logger.LogDebug("Inserting page view entity into repository");
        await _pageViewsRepository.InsertAsync(pageViewEntity, cancellationToken)
                                  .ConfigureAwait(false);
        _logger.LogDebug($"The id of the new page view is {pageViewEntity.Id}");

        return pageViewEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        var timestampThreshold = DateTime.UtcNow.Date.AddDays(-daysToKeep);
        _logger.LogDebug($"Deleting page views older than {timestampThreshold}");
        await _pageViewsRepository.DeleteAsync(x => x.Timestamp.Date < timestampThreshold, cancellationToken)
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
                                                               String spanId = null,
                                                               String traceId = null,
                                                               Uri url = null,
                                                               String userAgent = null,
                                                               CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Selecting page views from repository");
        var queryable = await _pageViewsRepository.SelectAsync(cancellationToken: cancellationToken)
                                                  .ConfigureAwait(false);

        /* Standard Filters */

        if (tenant.HasValue)
        {
            _logger.LogDebug($"Adding filter by tenant: {tenant}");
            queryable = queryable.Where(x => x.TenantId == tenant);
        }

        if (!String.IsNullOrEmpty(traceId))
        {
            _logger.LogDebug($"Adding filter by trace id: {traceId}");
            queryable = queryable.Where(x => x.TraceId == traceId);
        }

        if (!String.IsNullOrEmpty(spanId))
        {
            _logger.LogDebug($"Adding filter by span id: {spanId}");
            queryable = queryable.Where(x => x.SpanId == spanId);
        }

        if (dateTimeFrom.HasValue)
        {
            _logger.LogDebug($"Adding filter by timestamp: {dateTimeFrom.Value.ToUniversalTime()}");
            queryable = queryable.Where(x => x.Timestamp >= dateTimeFrom.Value.ToUniversalTime());
        }

        if (dateTimeTo.HasValue)
        {
            _logger.LogDebug($"Adding filter by timestamp: {dateTimeTo.Value.ToUniversalTime()}");
            queryable = queryable.Where(x => x.Timestamp <= dateTimeTo.Value.ToUniversalTime());
        }

        if (clientIpAddress != null)
        {
            _logger.LogDebug($"Adding filter by client ip address: {clientIpAddress}");
            queryable = queryable.Where(x => x.ClientIpAddress == clientIpAddress.ToString());
        }

        if (!String.IsNullOrEmpty(hostname))
        {
            _logger.LogDebug($"Adding filter by hostname: {hostname}");
            queryable = queryable.Where(x => x.HostName == hostname);
        }

        if (!String.IsNullOrEmpty(appName))
        {
            _logger.LogDebug($"Adding filter by application name: {appName}");
            queryable = queryable.Where(x => x.AppName == appName);
        }

        /* Custom Filters */

        if (!String.IsNullOrEmpty(deviceType))
        {
            _logger.LogDebug($"Adding filter by device type: {deviceType}");
            queryable = queryable.Where(x => x.DeviceType == deviceType);
        }

        if (!String.IsNullOrEmpty(name))
        {
            _logger.LogDebug($"Adding filter by name: {name}");
            queryable = queryable.Where(x => x.Name == name);
        }

        if (url != null)
        {
            _logger.LogDebug($"Adding filter by url: {url}");
            queryable = queryable.Where(x => x.Url == url.ToString());
        }

        if (!String.IsNullOrEmpty(userAgent))
        {
            _logger.LogDebug($"Adding filter by user agent: {userAgent}");
            queryable = queryable.Where(x => x.UserAgent == userAgent);
        }

        _logger.LogDebug("Executing query to retrieve page views from repository");
        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);
        _logger.LogDebug($"{listOfEntities.Count} page views was retrieved from the repository");

        return [.. listOfEntities.Select(x => x.ToPageViewDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_pageViewsPublisher == null)
        {
            throw new NotSupportedException("The page views publisher is not currently available");
        }

        _logger.LogDebug("Publishing a new page view message");
        var messageId = await _pageViewsPublisher.PublishAsync(deepSightDto, "pageviews", cancellationToken)
                                                 .ConfigureAwait(false);
        _logger.LogDebug($"The id of the published message is {messageId}");

        return messageId;
    }
}
