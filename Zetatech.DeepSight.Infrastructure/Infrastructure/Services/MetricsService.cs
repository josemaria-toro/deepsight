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

public sealed class MetricsService : BaseDeepSightService, IMetricsService
{
    private readonly IDeepSightPublisher _deepSightPublisher;
    private readonly IMetricsRepository _metricsRepository;

    public MetricsService(IDeepSightPublisher deepSightPublisher = null,
                          IMetricsRepository metricsRepository = null)
    {
        _deepSightPublisher = deepSightPublisher;
        _metricsRepository = metricsRepository;
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_metricsRepository == null)
        {
            throw new NotSupportedException("The metrics repository is not currently available");
        }

        var metricEntity = deepSightDto.ToMetricEntity();

        await _metricsRepository.InsertAsync(metricEntity, cancellationToken)
                                .ConfigureAwait(false);

        return metricEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        await _metricsRepository.DeleteAsync(x => x.Timestamp.Date < DateTime.UtcNow.AddDays(-daysToKeep), cancellationToken)
                                .ConfigureAwait(false);
    }
    public async Task<IList<MetricDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await GetUsingFiltersAsync(cancellationToken: cancellationToken);
    }
    public async Task<IList<MetricDto>> GetUsingFiltersAsync(String appName = null,
                                                             IPAddress clientIpAddress = null,
                                                             String hostname = null,
                                                             Guid? tenant = null,
                                                             DateTime? dateTimeFrom = null,
                                                             DateTime? dateTimeTo = null,
                                                             String dimension = null,
                                                             String name = null,
                                                             CancellationToken cancellationToken = default)
    {
        var queryable = await _metricsRepository.SelectAsync(cancellationToken: cancellationToken)
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

        if (!String.IsNullOrEmpty(name))
        {
            queryable = queryable.Where(x => x.Name == name);
        }

        if (!String.IsNullOrEmpty(dimension))
        {
            queryable = queryable.Where(x => x.Dimension == dimension);
        }

        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);

        return [.. listOfEntities.Select(x => x.ToMetricDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_deepSightPublisher == null)
        {
            throw new NotSupportedException("The metrics publisher is not currently available");
        }

        return await _deepSightPublisher.PublishAsync(deepSightDto, "metrics", cancellationToken)
                                        .ConfigureAwait(false);
    }
}