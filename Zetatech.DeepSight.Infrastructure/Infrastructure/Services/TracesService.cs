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

public sealed class TracesService : BaseDeepSightService, ITracesService
{
    private readonly ILogger _logger;
    private readonly IDeepSightPublisher _tracesPublisher;
    private readonly ITracesRepository _tracesRepository;

    public TracesService(ILoggerFactory loggerFactory,
                         IDeepSightPublisher tracesPublisher = null,
                         ITracesRepository tracesRepository = null)
    {
        _logger = loggerFactory.CreateLogger<TracesService>();
        _tracesPublisher = tracesPublisher;
        _tracesRepository = tracesRepository;
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_tracesRepository == null)
        {
            throw new NotSupportedException("The traces repository is not currently available");
        }

        _logger.LogDebug("Building trace entity from dto");
        var traceEntity = deepSightDto.ToTraceEntity();
        _logger.LogDebug("Inserting trace entity into repository");
        await _tracesRepository.InsertAsync(traceEntity, cancellationToken)
                               .ConfigureAwait(false);
        _logger.LogDebug($"The id of the new trace is {traceEntity.Id}");

        return traceEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        var timestampThreshold = DateTime.UtcNow.Date.AddDays(-daysToKeep);
        _logger.LogDebug($"Deleting traces older than {timestampThreshold}");
        await _tracesRepository.DeleteAsync(x => x.Timestamp.Date < timestampThreshold, cancellationToken)
                               .ConfigureAwait(false);
    }
    public async Task<IList<TraceDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await GetUsingFiltersAsync(cancellationToken: cancellationToken);
    }
    public async Task<IList<TraceDto>> GetUsingFiltersAsync(String appName = null,
                                                            IPAddress clientIpAddress = null,
                                                            String hostname = null,
                                                            Guid? tenant = null,
                                                            DateTime? dateTimeFrom = null,
                                                            DateTime? dateTimeTo = null,
                                                            String category = null,
                                                            String message = null,
                                                            LogLevel? severity = null,
                                                            String spanId = null,
                                                            String traceId = null,
                                                            CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Selecting traces from repository");
        var queryable = await _tracesRepository.SelectAsync(cancellationToken: cancellationToken)
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

        if (!String.IsNullOrEmpty(category))
        {
            _logger.LogDebug($"Adding filter by category: {category}");
            queryable = queryable.Where(x => x.Category == category);
        }

        if (!String.IsNullOrEmpty(message))
        {
            _logger.LogDebug($"Adding filter by message: {message}");
            queryable = queryable.Where(x => x.Message.Contains(message));
        }

        if (severity.HasValue)
        {
            _logger.LogDebug($"Adding filter by severity: {severity}");
            queryable = queryable.Where(x => x.Severity == severity.ToString());
        }

        _logger.LogDebug("Executing query to retrieve traces from repository");
        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);
        _logger.LogDebug($"{listOfEntities.Count} traces was retrieved from the repository");

        return [.. listOfEntities.Select(x => x.ToTraceDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_tracesPublisher == null)
        {
            throw new NotSupportedException("The traces publisher is not currently available");
        }

        _logger.LogDebug("Publishing a new trace message");
        var messageId = await _tracesPublisher.PublishAsync(deepSightDto, "traces", cancellationToken)
                                              .ConfigureAwait(false);
        _logger.LogDebug($"The id of the published message is {messageId}");

        return messageId;
    }
}
