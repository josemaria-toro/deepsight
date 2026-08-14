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

public sealed class RequestsService : BaseDeepSightService, IRequestsService
{
    private readonly IDeepSightPublisher _deepSightPublisher;
    private readonly IRequestsRepository _requestsRepository;

    public RequestsService(IDeepSightPublisher deepSightPublisher = null,
                           IRequestsRepository requestsRepository = null)
    {
        _deepSightPublisher = deepSightPublisher;
        _requestsRepository = requestsRepository;
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_requestsRepository == null)
        {
            throw new NotSupportedException("The requests repository is not currently available");
        }

        var requestEntity = deepSightDto.ToRequestEntity();

        await _requestsRepository.InsertAsync(requestEntity, cancellationToken)
                                 .ConfigureAwait(false);

        return requestEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        await _requestsRepository.DeleteAsync(x => x.Timestamp.Date < DateTime.UtcNow.AddDays(-daysToKeep), cancellationToken)
                                 .ConfigureAwait(false);
    }
    public async Task<IList<RequestDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await GetUsingFiltersAsync(cancellationToken: cancellationToken);
    }
    public async Task<IList<RequestDto>> GetUsingFiltersAsync(String appName = null,
                                                              IPAddress clientIpAddress = null,
                                                              String hostname = null,
                                                              Guid? tenant = null,
                                                              DateTime? dateTimeFrom = null,
                                                              DateTime? dateTimeTo = null,
                                                              Double? durationFrom = null,
                                                              Double? durationTo = null,
                                                              String endpoint = null,
                                                              IPAddress ipAddress = null,
                                                              String name = null,
                                                              Int32? statusCode = null,
                                                              Boolean? success = null,
                                                              String type = null,
                                                              CancellationToken cancellationToken = default)
    {
        var queryable = await _requestsRepository.SelectAsync(cancellationToken: cancellationToken)
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

        if (!String.IsNullOrEmpty(endpoint))
        {
            queryable = queryable.Where(x => x.EndPoint == endpoint);
        }

        if (ipAddress != null)
        {
            queryable = queryable.Where(x => x.IPAddress == ipAddress.ToString());
        }

        if (!String.IsNullOrEmpty(name))
        {
            queryable = queryable.Where(x => x.Name == name);
        }

        if (statusCode.HasValue)
        {
            queryable = queryable.Where(x => x.StatusCode == statusCode);
        }

        if (success.HasValue)
        {
            queryable = queryable.Where(x => x.Success == success);
        }

        if (!String.IsNullOrEmpty(type))
        {
            queryable = queryable.Where(x => x.Type == type);
        }

        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);

        return [.. listOfEntities.Select(x => x.ToRequestDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_deepSightPublisher == null)
        {
            throw new NotSupportedException("The requests publisher is not currently available");
        }

        return await _deepSightPublisher.PublishAsync(deepSightDto, "requests", cancellationToken)
                                        .ConfigureAwait(false);
    }
}