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
    private readonly IDeepSightPublisher _deepSightPublisher;
    private readonly ITracesRepository _tracesRepository;

    public TracesService(IDeepSightPublisher deepSightPublisher = null,
                         ITracesRepository tracesRepository = null)
    {
        _deepSightPublisher = deepSightPublisher;
        _tracesRepository = tracesRepository;
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_tracesRepository == null)
        {
            throw new NotSupportedException("The traces repository is not currently available");
        }

        var traceEntity = deepSightDto.ToTraceEntity();

        await _tracesRepository.InsertAsync(traceEntity, cancellationToken)
                               .ConfigureAwait(false);

        return traceEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        await _tracesRepository.DeleteAsync(x => x.Timestamp.Date < DateTime.UtcNow.AddDays(-daysToKeep), cancellationToken)
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
                                                            CancellationToken cancellationToken = default)
    {
        var queryable = await _tracesRepository.SelectAsync(cancellationToken: cancellationToken)
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

        if (!String.IsNullOrEmpty(category))
        {
            queryable = queryable.Where(x => x.Category == category);
        }

        if (!String.IsNullOrEmpty(message))
        {
            queryable = queryable.Where(x => x.Message.Contains(message));
        }

        if (severity.HasValue)
        {
            queryable = queryable.Where(x => x.Severity == severity.ToString());
        }

        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);

        return [.. listOfEntities.Select(x => x.ToTraceDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_deepSightPublisher == null)
        {
            throw new NotSupportedException("The traces publisher is not currently available");
        }

        return await _deepSightPublisher.PublishAsync(deepSightDto, "traces", cancellationToken)
                                        .ConfigureAwait(false);
    }
}