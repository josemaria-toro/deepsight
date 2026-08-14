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

public sealed class EventsService : BaseDeepSightService, IEventsService
{
    private readonly IDeepSightPublisher _deepSightPublisher;
    private readonly IEventsRepository _eventsRepository;

    public EventsService(IDeepSightPublisher deepSightPublisher = null,
                         IEventsRepository eventsRepository = null)
    {
        _deepSightPublisher = deepSightPublisher;
        _eventsRepository = eventsRepository;
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_eventsRepository == null)
        {
            throw new NotSupportedException("The events repository is not currently available");
        }

        var eventEntity = deepSightDto.ToEventEntity();

        await _eventsRepository.InsertAsync(eventEntity, cancellationToken)
                               .ConfigureAwait(false);

        return eventEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        await _eventsRepository.DeleteAsync(x => x.Timestamp.Date < DateTime.UtcNow.AddDays(-daysToKeep), cancellationToken)
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
                                                            CancellationToken cancellationToken = default)
    {
        var queryable = await _eventsRepository.SelectAsync(cancellationToken: cancellationToken)
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

        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);

        return [.. listOfEntities.Select(x => x.ToEventDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_deepSightPublisher == null)
        {
            throw new NotSupportedException("The events publisher is not currently available");
        }

        return await _deepSightPublisher.PublishAsync(deepSightDto, "events", cancellationToken)
                                        .ConfigureAwait(false);
    }
}