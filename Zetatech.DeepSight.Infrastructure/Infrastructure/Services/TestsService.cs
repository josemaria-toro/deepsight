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

public sealed class TestsService : BaseDeepSightService, ITestsService
{
    private readonly IDeepSightPublisher _deepSightPublisher;
    private readonly ITestsRepository _testsRepository;

    public TestsService(IDeepSightPublisher deepSightPublisher = null,
                        ITestsRepository testsRepository = null)
    {
        _deepSightPublisher = deepSightPublisher;
        _testsRepository = testsRepository;
    }

    public override async Task<Guid> CreateAsync(DeepSightDto deepSightDto,
                                                 CancellationToken cancellationToken = default)
    {
        if (_testsRepository == null)
        {
            throw new NotSupportedException("The tests repository is not currently available");
        }

        var testEntity = deepSightDto.ToTestEntity();

        await _testsRepository.InsertAsync(testEntity, cancellationToken)
                               .ConfigureAwait(false);

        return testEntity.Id;
    }
    public override async Task DeleteAsync(UInt32 daysToKeep,
                                           CancellationToken cancellationToken = default)
    {
        await _testsRepository.DeleteAsync(x => x.Timestamp.Date < DateTime.UtcNow.AddDays(-daysToKeep), cancellationToken)
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
                                                           Boolean? success = null,
                                                           CancellationToken cancellationToken = default)
    {
        var queryable = await _testsRepository.SelectAsync(cancellationToken: cancellationToken)
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

        if (!String.IsNullOrEmpty(message))
        {
            queryable = queryable.Where(x => x.Message.Contains(message));
        }

        if (!String.IsNullOrEmpty(name))
        {
            queryable = queryable.Where(x => x.Name == name);
        }

        if (success.HasValue)
        {
            queryable = queryable.Where(x => x.Success == success);
        }

        var listOfEntities = await queryable.OrderByDescending(x => x.Timestamp)
                                            .ToListAsync(cancellationToken)
                                            .ConfigureAwait(false);

        return [.. listOfEntities.Select(x => x.ToTestDto())];
    }
    public override async Task<Guid> PublishAsync(DeepSightDto deepSightDto,
                                                  CancellationToken cancellationToken = default)
    {
        if (_deepSightPublisher == null)
        {
            throw new NotSupportedException("The tests publisher is not currently available");
        }

        return await _deepSightPublisher.PublishAsync(deepSightDto, "tests", cancellationToken)
                                        .ConfigureAwait(false);
    }
}