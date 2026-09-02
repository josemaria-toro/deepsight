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

    public DependenciesService(ILoggerFactory loggerFactory,
                               IDeepSightPublisher dependenciesPublisher = null,
                               IDependenciesRepository dependenciesRepository = null) : base(loggerFactory)
    {
        _dependenciesPublisher = dependenciesPublisher;
        _dependenciesRepository = dependenciesRepository;
    }

    public async Task<Guid> CreateAsync(DependencyDto dependencyDto, CancellationToken cancellationToken = default)
    {
        if (_dependenciesRepository == null)
        {
            throw new NotSupportedException("The dependencies repository is not currently available");
        }

        Logger.LogDebug("Building entity from dto");
        var dependencyEntity = dependencyDto.Build();
        Logger.LogDebug("Inserting entity into database");
        await _dependenciesRepository.InsertAsync(dependencyEntity, cancellationToken)
                                     .ConfigureAwait(false);
        Logger.LogDebug($"The id of the new dependency is {dependencyEntity.Id}");

        return dependencyEntity.Id;
    }
    public async Task DeleteAsync(UInt32 daysToKeep, CancellationToken cancellationToken = default)
    {
        var threshold = DateTime.UtcNow.Date.AddDays(-daysToKeep);
        Logger.LogDebug($"Deleting dependencies older than {threshold}");
        await _dependenciesRepository.DeleteAsync(x => x.Timestamp.Date < threshold, cancellationToken)
                                     .ConfigureAwait(false);
    }
    public async Task<IList<DependencyDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await SearchAsync(cancellationToken: cancellationToken);
    }
    public async Task<Guid> PublishAsync(DependencyDto dependencyDto, CancellationToken cancellationToken = default)
    {
        if (_dependenciesPublisher == null)
        {
            throw new NotSupportedException("The dependencies publisher is not currently available");
        }

        Logger.LogDebug("Publishing a new dependency message");
        var messageId = await _dependenciesPublisher.PublishAsync(dependencyDto, "dependencies", cancellationToken)
                                                    .ConfigureAwait(false);
        Logger.LogDebug($"The id of the published message is {messageId}");

        return messageId;
    }
    public async Task<IList<DependencyDto>> SearchAsync(String appName = null,
                                                        IPAddress clientIpAddress = null,
                                                        DateTime? dateTimeFrom = null,
                                                        DateTime? dateTimeTo = null,
                                                        Double? durationFrom = null,
                                                        Double? durationTo = null,
                                                        String hostName = null,
                                                        String name = null,
                                                        String spanId = null,
                                                        Boolean? success = null,
                                                        String target = null,
                                                        Guid? tenantId = null,
                                                        String traceId = null,
                                                        String type = null,
                                                        CancellationToken cancellationToken = default)
    {
        Logger.LogDebug("Selecting dependencies from database");
        var queryable = await _dependenciesRepository.SelectAsync(cancellationToken: cancellationToken)
                                                     .ConfigureAwait(false);

        /* Standard Filters */

        if (tenantId.HasValue)
        {
            Logger.LogDebug($"Adding filter by tenant: {tenantId}");
            queryable = queryable.Where(x => x.TenantId == tenantId);
        }

        if (!String.IsNullOrEmpty(traceId))
        {
            Logger.LogDebug($"Adding filter by trace id: {traceId}");
            queryable = queryable.Where(x => x.TraceId == traceId);
        }

        if (!String.IsNullOrEmpty(spanId))
        {
            Logger.LogDebug($"Adding filter by span id: {spanId}");
            queryable = queryable.Where(x => x.SpanId == spanId);
        }

        if (dateTimeFrom.HasValue)
        {
            Logger.LogDebug($"Adding filter by timestamp: {dateTimeFrom.Value.ToUniversalTime()}");
            queryable = queryable.Where(x => x.Timestamp >= dateTimeFrom.Value.ToUniversalTime());
        }

        if (dateTimeTo.HasValue)
        {
            Logger.LogDebug($"Adding filter by timestamp: {dateTimeTo.Value.ToUniversalTime()}");
            queryable = queryable.Where(x => x.Timestamp <= dateTimeTo.Value.ToUniversalTime());
        }

        if (clientIpAddress != null)
        {
            Logger.LogDebug($"Adding filter by client ip address: {clientIpAddress}");
            queryable = queryable.Where(x => x.ClientIpAddress == clientIpAddress.ToString());
        }

        if (!String.IsNullOrEmpty(hostName))
        {
            Logger.LogDebug($"Adding filter by hostname: {hostName}");
            queryable = queryable.Where(x => x.HostName == hostName);
        }

        if (!String.IsNullOrEmpty(appName))
        {
            Logger.LogDebug($"Adding filter by application name: {appName}");
            queryable = queryable.Where(x => x.AppName == appName);
        }

        /* Custom Filters */

        if (durationFrom.HasValue)
        {
            Logger.LogDebug($"Adding filter by duration: {durationFrom}");
            queryable = queryable.Where(x => x.Duration >= durationFrom);
        }

        if (durationTo.HasValue)
        {
            Logger.LogDebug($"Adding filter by duration: {durationTo}");
            queryable = queryable.Where(x => x.Duration <= durationTo);
        }

        if (!String.IsNullOrEmpty(name))
        {
            Logger.LogDebug($"Adding filter by name: {name}");
            queryable = queryable.Where(x => x.Name == name);
        }

        if (!String.IsNullOrEmpty(type))
        {
            Logger.LogDebug($"Adding filter by type: {type}");
            queryable = queryable.Where(x => x.Type == type);
        }

        if (success.HasValue)
        {
            Logger.LogDebug($"Adding filter by result: {success}");
            queryable = queryable.Where(x => x.Success == success);
        }

        if (!String.IsNullOrEmpty(target))
        {
            Logger.LogDebug($"Adding filter by target: {target}");
            queryable = queryable.Where(x => x.Target == target);
        }

        Logger.LogDebug("Executing query to retrieve dependencies from database");
        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);
        Logger.LogDebug($"{listOfEntities.Count} dependencies was retrieved from the database");

        return [.. listOfEntities.Select(x => x.Build())];
    }
}
