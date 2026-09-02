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

public sealed class ErrorsService : BaseDeepSightService, IErrorsService
{
    private readonly IDeepSightPublisher _errorsPublisher;
    private readonly IErrorsRepository _errorsRepository;
    private readonly ILogger _logger;

    public ErrorsService(ILoggerFactory loggerFactory,
                         IDeepSightPublisher errorsPublisher = null,
                         IErrorsRepository errorsRepository = null)
    {
        _errorsPublisher = errorsPublisher;
        _errorsRepository = errorsRepository;
        _logger = loggerFactory.CreateLogger<ErrorsService>();
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_errorsRepository == null)
        {
            throw new NotSupportedException("The errors repository is not currently available");
        }

        _logger.LogDebug("Building error entity from dto");
        var errorEntity = deepSightDto.ToErrorDto();
        _logger.LogDebug("Inserting error entity into repository");
        await _errorsRepository.InsertAsync(errorEntity, cancellationToken)
                               .ConfigureAwait(false);
        _logger.LogDebug($"The id of the new error is {errorEntity.Id}");

        return errorEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        var timestampThreshold = DateTime.UtcNow.Date.AddDays(-daysToKeep);
        _logger.LogDebug($"Deleting errors older than {timestampThreshold}");
        await _errorsRepository.DeleteAsync(x => x.Timestamp.Date < timestampThreshold, cancellationToken)
                               .ConfigureAwait(false);
    }
    public async Task<IList<ErrorDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await GetUsingFiltersAsync(cancellationToken: cancellationToken);
    }
    public async Task<IList<ErrorDto>> GetUsingFiltersAsync(String appName = null,
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
                                                            String type = null,
                                                            CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Selecting errors from repository");
        var queryable = await _errorsRepository.SelectAsync(cancellationToken: cancellationToken)
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

        if (!String.IsNullOrEmpty(type))
        {
            _logger.LogDebug($"Adding filter by type: {type}");
            queryable = queryable.Where(x => x.Type == type);
        }

        _logger.LogDebug("Executing query to retrieve errors from repository");
        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);
        _logger.LogDebug($"{listOfEntities.Count} errors was retrieved from the repository");

        return [.. listOfEntities.Select(x => x.ToErrorDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_errorsPublisher == null)
        {
            throw new NotSupportedException("The errors publisher is not currently available");
        }

        _logger.LogDebug("Publishing a new error message");
        var messageId = await _errorsPublisher.PublishAsync(deepSightDto, "errors", cancellationToken)
                                              .ConfigureAwait(false);
        _logger.LogDebug($"The id of the published message is {messageId}");

        return messageId;
    }
}
