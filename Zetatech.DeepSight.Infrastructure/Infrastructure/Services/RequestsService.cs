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

public sealed class RequestsService : BaseDeepSightService, IRequestsService
{
    private readonly ILogger _logger;
    private readonly IDeepSightPublisher _requestsPublisher;
    private readonly IRequestsRepository _requestsRepository;

    public RequestsService(ILoggerFactory loggerFactory,
                           IDeepSightPublisher requestsPublisher = null,
                           IRequestsRepository requestsRepository = null) : base(loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<RequestsService>();
        _requestsPublisher = requestsPublisher;
        _requestsRepository = requestsRepository;
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_requestsRepository == null)
        {
            throw new NotSupportedException("The requests repository is not currently available");
        }

        _logger.LogDebug("Building request entity from dto");
        var requestEntity = deepSightDto.ToRequestEntity();
        _logger.LogDebug("Inserting request entity into repository");
        await _requestsRepository.InsertAsync(requestEntity, cancellationToken)
                                 .ConfigureAwait(false);
        _logger.LogDebug($"The id of the new request is {requestEntity.Id}");

        return requestEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        var timestampThreshold = DateTime.UtcNow.Date.AddDays(-daysToKeep);
        _logger.LogDebug($"Deleting requests older than {timestampThreshold}");
        await _requestsRepository.DeleteAsync(x => x.Timestamp.Date < timestampThreshold, cancellationToken)
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
                                                              String spanId = null,
                                                              Int32? statusCode = null,
                                                              Boolean? success = null,
                                                              String traceId = null,
                                                              String type = null,
                                                              CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Selecting requests from repository");
        var queryable = await _requestsRepository.SelectAsync(cancellationToken: cancellationToken)
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

        if (!String.IsNullOrEmpty(endpoint))
        {
            _logger.LogDebug($"Adding filter by endpoint: {endpoint}");
            queryable = queryable.Where(x => x.EndPoint == endpoint);
        }

        if (ipAddress != null)
        {
            _logger.LogDebug($"Adding filter by remote ip address: {ipAddress}");
            queryable = queryable.Where(x => x.IPAddress == ipAddress.ToString());
        }

        if (!String.IsNullOrEmpty(name))
        {
            _logger.LogDebug($"Adding filter by name: {name}");
            queryable = queryable.Where(x => x.Name == name);
        }

        if (statusCode.HasValue)
        {
            _logger.LogDebug($"Adding filter by status code: {statusCode}");
            queryable = queryable.Where(x => x.StatusCode == statusCode);
        }

        if (success.HasValue)
        {
            _logger.LogDebug($"Adding filter by result: {success}");
            queryable = queryable.Where(x => x.Success == success);
        }

        if (!String.IsNullOrEmpty(type))
        {
            _logger.LogDebug($"Adding filter by type: {type}");
            queryable = queryable.Where(x => x.Type == type);
        }

        _logger.LogDebug("Executing query to retrieve requests from repository");
        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);
        _logger.LogDebug($"{listOfEntities.Count} requests was retrieved from the repository");

        return [.. listOfEntities.Select(x => x.ToRequestDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_requestsPublisher == null)
        {
            throw new NotSupportedException("The requests publisher is not currently available");
        }

        _logger.LogDebug("Publishing a new request message");
        var messageId = await _requestsPublisher.PublishAsync(deepSightDto, "requests", cancellationToken)
                                                .ConfigureAwait(false);
        _logger.LogDebug($"The id of the published message is {messageId}");

        return messageId;
    }
}
