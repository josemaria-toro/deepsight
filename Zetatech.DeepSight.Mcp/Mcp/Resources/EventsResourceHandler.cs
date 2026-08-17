using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class EventsResourceHandler
{
    private readonly IEventsService _eventsService;
    private readonly ILogger _logger;

    public EventsResourceHandler(IEventsService eventsService,
                                 ILoggerFactory loggerFactory)
    {
        _eventsService = eventsService;
        _logger = loggerFactory.CreateLogger<EventsResourceHandler>();
    }

    [McpServerResource(UriTemplate = "db://events")]
    public async Task<String> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Se obtienen los eventos");
        return "Estos son tus eventos";
    }
}