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

public sealed class EventsService : BaseDeepSightService, IEventsService
{
    private readonly IDeepSightPublisher _eventsPublisher;
    private readonly IEventsRepository _eventsRepository;
    private readonly ILogger _logger;

    public EventsService(ILoggerFactory loggerFactory,
                         IDeepSightPublisher eventsPublisher = null,
                         IEventsRepository eventsRepository = null) : base(loggerFactory)
    {
        _eventsPublisher = eventsPublisher;
        _eventsRepository = eventsRepository;
        _logger = loggerFactory.CreateLogger<EventsService>();
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_eventsRepository == null)
        {
            throw new NotSupportedException("The events repository is not currently available");
        }

        _logger.LogDebug("Building event entity from dto");
        var eventEntity = deepSightDto.ToEventEntity();
        _logger.LogDebug("Inserting event entity into repository");
        await _eventsRepository.InsertAsync(eventEntity, cancellationToken)
                               .ConfigureAwait(false);
        _logger.LogDebug($"The id of the new event is {eventEntity.Id}");

        return eventEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        var timestampThreshold = DateTime.UtcNow.Date.AddDays(-daysToKeep);
        _logger.LogDebug($"Deleting events older than {timestampThreshold}");
        await _eventsRepository.DeleteAsync(x => x.Timestamp.Date < timestampThreshold, cancellationToken)
                               .ConfigureAwait(false);
    }
    public async Task<IList<EventDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await GetUsingFiltersAsync(cancellationToken: cancellationToken);
    }
    public async Task<IList<EventDto>> GetUsingFiltersAsync(String appName = null,
                                                            IPAddress clientIpAddress = null,
                                                            String hostname = null,
                                                            Guid? tenant = null,
                                                            DateTime? dateTimeFrom = null,
                                                            DateTime? dateTimeTo = null,
                                                            String name = null,
                                                            String spanId = null,
                                                            String traceId = null,
                                                            CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Selecting events from repository");
        var queryable = await _eventsRepository.SelectAsync(cancellationToken: cancellationToken)
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

        if (!String.IsNullOrEmpty(name))
        {
            _logger.LogDebug($"Adding filter by name: {name}");
            queryable = queryable.Where(x => x.Name == name);
        }

        _logger.LogDebug("Executing query to retrieve events from repository");
        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);
        _logger.LogDebug($"{listOfEntities.Count} events was retrieved from the repository");

        return [.. listOfEntities.Select(x => x.ToEventDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_eventsPublisher == null)
        {
            throw new NotSupportedException("The events publisher is not currently available");
        }

        _logger.LogDebug("Publishing a new event message");
        var messageId = await _eventsPublisher.PublishAsync(deepSightDto, "events", cancellationToken)
                                              .ConfigureAwait(false);
        _logger.LogDebug($"The id of the published message is {messageId}");

        return messageId;
    }
}
