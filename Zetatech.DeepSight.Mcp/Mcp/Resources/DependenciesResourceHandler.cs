using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class DependenciesResourceHandler
{
    private readonly IDependenciesService _dependenciesService;
    private readonly ILogger _logger;

    public DependenciesResourceHandler(IDependenciesService dependenciesService,
                                       ILoggerFactory loggerFactory)
    {
        _dependenciesService = dependenciesService;
        _logger = loggerFactory.CreateLogger<DependenciesResourceHandler>();
    }

    [McpServerResource(UriTemplate = "db://dependencies")]
    public async Task<String> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Se obtienen las dependencias");
        return "Estas son tus dependencias";
    }
}