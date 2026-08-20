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

public sealed class TestsService : BaseDeepSightService, ITestsService
{
    private readonly ILogger _logger;
    private readonly IDeepSightPublisher _testsPublisher;
    private readonly ITestsRepository _testsRepository;

    public TestsService(ILoggerFactory loggerFactory,
                        IDeepSightPublisher testsPublisher = null,
                        ITestsRepository testsRepository = null)
    {
        _logger = loggerFactory.CreateLogger<TestsService>();
        _testsPublisher = testsPublisher;
        _testsRepository = testsRepository;
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_testsRepository == null)
        {
            throw new NotSupportedException("The tests repository is not currently available");
        }

        _logger.LogDebug("Building test entity from dto");
        var testEntity = deepSightDto.ToTestEntity();
        _logger.LogDebug("Inserting test entity into repository");
        await _testsRepository.InsertAsync(testEntity, cancellationToken)
                               .ConfigureAwait(false);
        _logger.LogDebug($"The id of the new test is {testEntity.Id}");

        return testEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        var timestampThreshold = DateTime.UtcNow.Date.AddDays(-daysToKeep);
        _logger.LogDebug($"Deleting tests older than {timestampThreshold}");
        await _testsRepository.DeleteAsync(x => x.Timestamp.Date < timestampThreshold, cancellationToken)
                              .ConfigureAwait(false);
    }
    public async Task<IList<TestDto>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await GetUsingFiltersAsync(cancellationToken: cancellationToken);
    }
    public async Task<IList<TestDto>> GetUsingFiltersAsync(String appName = null,
                                                           IPAddress clientIpAddress = null,
                                                           String hostname = null,
                                                           Guid? tenant = null,
                                                           DateTime? dateTimeFrom = null,
                                                           DateTime? dateTimeTo = null,
                                                           Double? durationFrom = null,
                                                           Double? durationTo = null,
                                                           String message = null,
                                                           String name = null,
                                                           String spanId = null,
                                                           Boolean? success = null,
                                                           String traceId = null,
                                                           CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Selecting tests from repository");
        var queryable = await _testsRepository.SelectAsync(cancellationToken: cancellationToken)
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

        if (!String.IsNullOrEmpty(message))
        {
            _logger.LogDebug($"Adding filter by message: {message}");
            queryable = queryable.Where(x => x.Message.Contains(message));
        }

        if (!String.IsNullOrEmpty(name))
        {
            _logger.LogDebug($"Adding filter by name: {name}");
            queryable = queryable.Where(x => x.Name == name);
        }

        if (success.HasValue)
        {
            _logger.LogDebug($"Adding filter by result: {success}");
            queryable = queryable.Where(x => x.Success == success);
        }

        _logger.LogDebug("Executing query to retrieve tests from repository");
        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);
        _logger.LogDebug($"{listOfEntities.Count} tests was retrieved from the repository");

        return [.. listOfEntities.Select(x => x.ToTestDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_testsPublisher == null)
        {
            throw new NotSupportedException("The tests publisher is not currently available");
        }

        _logger.LogDebug("Publishing a new test message");
        var messageId = await _testsPublisher.PublishAsync(deepSightDto, "tests", cancellationToken)
                                             .ConfigureAwait(false);
        _logger.LogDebug($"The id of the published message is {messageId}");

        return messageId;
    }
}
