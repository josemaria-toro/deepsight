using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class MetricsResourceHandler
{
    private readonly ILogger _logger;
    private readonly IMetricsService _metricsService;

    public MetricsResourceHandler(IMetricsService metricsService,
                                  ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<MetricsResourceHandler>();
        _metricsService = metricsService;
    }

    [McpServerResource(UriTemplate = "db://metrics")]
    public async Task<String> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Se obtienen las métricas");
        return "Estas son tus métricas";
    }
}