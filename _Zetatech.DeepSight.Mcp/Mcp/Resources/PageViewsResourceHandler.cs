using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol.Server;
using Zetatech.Accelerate.Serialization;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class PageViewsResourceHandler
{
    private readonly IPageViewsService _pageViewsService;

    public PageViewsResourceHandler(IPageViewsService pageViewsService)
    {
        _pageViewsService = pageViewsService;
    }

    [McpServerResource(UriTemplate = "db://pageviews{?appName,clientIpAddress,hostname,tenant,dateTimeFrom,dateTimeTo,deviceType,name,spanId,traceId,url,userAgent}", MimeType = "application/json")]
    public async Task<String> GetAsync(String appName = null,
                                       IPAddress clientIpAddress = null,
                                       String hostname = null,
                                       Guid? tenant = null,
                                       DateTime? dateTimeFrom = null,
                                       DateTime? dateTimeTo = null,
                                       String deviceType = null,
                                       String name = null,
                                       String spanId = null,
                                       String traceId = null,
                                       Uri url = null,
                                       String userAgent = null,
                                       CancellationToken cancellationToken = default)
    {
        var pageViewDtos = await _pageViewsService.GetUsingFiltersAsync(appName, clientIpAddress, hostname, tenant, dateTimeFrom, dateTimeTo, deviceType, name, spanId, traceId, url, userAgent, cancellationToken)
                                                  .ConfigureAwait(false);

        return Json.ToString(pageViewDtos);
    }
}
