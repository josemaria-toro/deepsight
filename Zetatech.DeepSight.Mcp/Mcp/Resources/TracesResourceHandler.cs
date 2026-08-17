using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Zetatech.DeepSight.Application.Dtos;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class TracesResourceHandler
{
    private readonly ILogger _logger;
    private readonly ITracesService _tracesService;

    public TracesResourceHandler(ITracesService tracesService,
                                 ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TracesResourceHandler>();
        _tracesService = tracesService;
    }

    [McpServerResource(UriTemplate = "db://traces{?appName,clientIpAddress,hostname,tenant,dateTimeFrom,dateTimeTo,category,message,severity}")]
    public async Task<IList<TraceDto>> GetAsync(String appName = null,
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
        return await _tracesService.GetUsingFiltersAsync(appName,
                                                         clientIpAddress,
                                                         hostname,
                                                         tenant,
                                                         dateTimeFrom,
                                                         dateTimeTo,
                                                         category,
                                                         message,
                                                         severity,
                                                         cancellationToken: cancellationToken)
                                   .ConfigureAwait(false);
    }
}