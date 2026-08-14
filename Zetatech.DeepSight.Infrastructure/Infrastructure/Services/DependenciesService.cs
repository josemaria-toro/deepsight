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

public sealed class DependenciesService : BaseDeepSightService, IDependenciesService
{
    private readonly IDeepSightPublisher _deepSightPublisher;
    private readonly IDependenciesRepository _dependenciesRepository;

    public DependenciesService(IDeepSightPublisher deepSightPublisher = null,
                               IDependenciesRepository dependenciesRepository = null)
    {
        _deepSightPublisher = deepSightPublisher;
        _dependenciesRepository = dependenciesRepository;
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_dependenciesRepository == null)
        {
            throw new NotSupportedException("The dependencies repository is not currently available");
        }

        var dependencyEntity = deepSightDto.ToDependencyEntity();

        await _dependenciesRepository.InsertAsync(dependencyEntity, cancellationToken)
                                     .ConfigureAwait(false);

        return dependencyEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        await _dependenciesRepository.DeleteAsync(x => x.Timestamp.Date < DateTime.UtcNow.Date.AddDays(-daysToKeep), cancellationToken)
                                     .ConfigureAwait(false);
    }
    public async Task<IList<DependencyDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await GetUsingFiltersAsync(cancellationToken: cancellationToken);
    }
    public async Task<IList<DependencyDto>> GetUsingFiltersAsync(String appName = null,
                                                                 IPAddress clientIpAddress = null,
                                                                 String hostname = null,
                                                                 Guid? tenant = null,
                                                                 DateTime? dateTimeFrom = null,
                                                                 DateTime? dateTimeTo = null,
                                                                 Double? durationFrom = null,
                                                                 Double? durationTo = null,
                                                                 String name = null,
                                                                 Boolean? success = null,
                                                                 String target = null,
                                                                 String type = null,
                                                                 CancellationToken cancellationToken = default)
    {
        var queryable = await _dependenciesRepository.SelectAsync(cancellationToken: cancellationToken)
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

        if (durationFrom.HasValue)
        {
            queryable = queryable.Where(x => x.Duration >= durationFrom);
        }

        if (durationTo.HasValue)
        {
            queryable = queryable.Where(x => x.Duration <= durationTo);
        }

        if (!String.IsNullOrEmpty(name))
        {
            queryable = queryable.Where(x => x.Name == name);
        }

        if (!String.IsNullOrEmpty(type))
        {
            queryable = queryable.Where(x => x.Type == type);
        }

        if (success.HasValue)
        {
            queryable = queryable.Where(x => x.Success == success);
        }

        if (!String.IsNullOrEmpty(target))
        {
            queryable = queryable.Where(x => x.Target == target);
        }

        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);

        return [.. listOfEntities.Select(x => x.ToDependencyDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_deepSightPublisher == null)
        {
            throw new NotSupportedException("The dependencies publisher is not currently available");
        }

        return await _deepSightPublisher.PublishAsync(deepSightDto, "dependencies", cancellationToken)
                                        .ConfigureAwait(false);
    }
}
