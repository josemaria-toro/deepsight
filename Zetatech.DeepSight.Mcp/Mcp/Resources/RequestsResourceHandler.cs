using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class RequestsResourceHandler
{
    private readonly ILogger _logger;
    private readonly IRequestsService _requestsService;

    public RequestsResourceHandler(IRequestsService requestsService,
                                   ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<RequestsResourceHandler>();
        _requestsService = requestsService;
    }

    [McpServerResource(UriTemplate = "db://requests")]
    public async Task<String> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Se obtienen las peticiones");
        return "Estas son tus peticiones";
    }
}