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

public sealed class DependenciesService : BaseDeepSightService, IDependenciesService
{
    private readonly IDeepSightPublisher _dependenciesPublisher;
    private readonly IDependenciesRepository _dependenciesRepository;
    private readonly ILogger _logger;

    public DependenciesService(ILoggerFactory loggerFactory,
                               IDeepSightPublisher dependenciesPublisher = null,
                               IDependenciesRepository dependenciesRepository = null)
    {
        _dependenciesPublisher = dependenciesPublisher;
        _dependenciesRepository = dependenciesRepository;
        _logger = loggerFactory.CreateLogger<DependenciesService>();
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_dependenciesRepository == null)
        {
            throw new NotSupportedException("The dependencies repository is not currently available");
        }

        _logger.LogDebug("Building dependency entity from dto");
        var dependencyEntity = deepSightDto.ToDependencyEntity();
        _logger.LogDebug("Inserting dependency entity into repository");
        await _dependenciesRepository.InsertAsync(dependencyEntity, cancellationToken)
                                     .ConfigureAwait(false);
        _logger.LogDebug($"The id of the new dependency is {dependencyEntity.Id}");

        return dependencyEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        var timestampThreshold = DateTime.UtcNow.Date.AddDays(-daysToKeep);
        _logger.LogDebug($"Deleting dependencies older than {timestampThreshold}");
        await _dependenciesRepository.DeleteAsync(x => x.Timestamp.Date < timestampThreshold, cancellationToken)
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
                                                                 String spanId = null,
                                                                 Boolean? success = null,
                                                                 String target = null,
                                                                 String traceId = null,
                                                                 String type = null,
                                                                 CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Selecting dependencies from repository");
        var queryable = await _dependenciesRepository.SelectAsync(cancellationToken: cancellationToken)
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

        if (durationFrom.HasValue)
        {
            _logger.LogDebug($"Adding filter by duration: {durationFrom}");
            queryable = queryable.Where(x => x.Duration >= durationFrom);
        }

        if (durationTo.HasValue)
        {
            _logger.LogDebug($"Adding filter by duration: {durationTo}");
            queryable = queryable.Where(x => x.Duration <= durationTo);
        }

        if (!String.IsNullOrEmpty(name))
        {
            _logger.LogDebug($"Adding filter by name: {name}");
            queryable = queryable.Where(x => x.Name == name);
        }

        if (!String.IsNullOrEmpty(type))
        {
            _logger.LogDebug($"Adding filter by type: {type}");
            queryable = queryable.Where(x => x.Type == type);
        }

        if (success.HasValue)
        {
            _logger.LogDebug($"Adding filter by result: {success}");
            queryable = queryable.Where(x => x.Success == success);
        }

        if (!String.IsNullOrEmpty(target))
        {
            _logger.LogDebug($"Adding filter by target: {target}");
            queryable = queryable.Where(x => x.Target == target);
        }

        _logger.LogDebug("Executing query to retrieve dependencies from repository");
        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);
        _logger.LogDebug($"{listOfEntities.Count} dependencies was retrieved from the repository");

        return [.. listOfEntities.Select(x => x.ToDependencyDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_dependenciesPublisher == null)
        {
            throw new NotSupportedException("The dependencies publisher is not currently available");
        }

        _logger.LogDebug("Publishing a new dependency message");
        var messageId = await _dependenciesPublisher.PublishAsync(deepSightDto, "dependencies", cancellationToken)
                                                    .ConfigureAwait(false);
        _logger.LogDebug($"The id of the published message is {messageId}");

        return messageId;
    }
}
