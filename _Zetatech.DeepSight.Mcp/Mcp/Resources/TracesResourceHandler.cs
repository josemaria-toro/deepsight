using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Zetatech.Accelerate.Serialization;
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

    [McpServerResource(UriTemplate = "db://traces{?appName,clientIpAddress,hostname,tenant,dateTimeFrom,dateTimeTo,category,message,severity,spanId,traceId}", MimeType = "application/json")]
    public async Task<String> GetAsync(String appName = null,
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
        var traceDtos = await _tracesService.GetUsingFiltersAsync(appName, clientIpAddress, hostname, tenant, dateTimeFrom, dateTimeTo, category, message, severity, spanId, traceId, cancellationToken)
                                            .ConfigureAwait(false);

        return Json.ToString(traceDtos);
    }
}
