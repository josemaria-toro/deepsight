using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class PageViewsResourceHandler
{
    private readonly ILogger _logger;
    private readonly IPageViewsService _pageViewsService;

    public PageViewsResourceHandler(IPageViewsService pageViewsService,
                                    ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<PageViewsResourceHandler>();
        _pageViewsService = pageViewsService;
    }

    [McpServerResource(UriTemplate = "db://pageviews")]
    public async Task<String> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Se obtienen las visualizaciones de páginas");
        return "Estas son tus visualizaciones de páginas";
    }
}