using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol.Server;
using Zetatech.Accelerate.Serialization;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class MetricsResourceHandler
{
    private readonly IMetricsService _metricsService;

    public MetricsResourceHandler(IMetricsService metricsService)
    {
        _metricsService = metricsService;
    }

    [McpServerResource(UriTemplate = "db://metrics{?appName,clientIpAddress,hostname,tenant,dateTimeFrom,dateTimeTo,dimension,name,spanId,traceId}", MimeType = "application/json")]
    public async Task<String> GetAsync(String appName = null,
                                       IPAddress clientIpAddress = null,
                                       String hostname = null,
                                       Guid? tenant = null,
                                       DateTime? dateTimeFrom = null,
                                       DateTime? dateTimeTo = null,
                                       String dimension = null,
                                       String name = null,
                                       String spanId = null,
                                       String traceId = null,
                                       CancellationToken cancellationToken = default)
    {
        var metricDtos = await _metricsService.GetUsingFiltersAsync(appName, clientIpAddress, hostname, tenant, dateTimeFrom, dateTimeTo, dimension, name, spanId, traceId, cancellationToken)
                                              .ConfigureAwait(false);

        return Json.ToString(metricDtos);
    }
}
