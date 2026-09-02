using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol.Server;
using Zetatech.Accelerate.Serialization;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class EventsResourceHandler
{
    private readonly IEventsService _eventsService;

    public EventsResourceHandler(IEventsService eventsService)
    {
        _eventsService = eventsService;
    }

    [McpServerResource(UriTemplate = "db://events{?appName,clientIpAddress,hostname,tenant,dateTimeFrom,dateTimeTo,name,spanId,traceId}", MimeType = "application/json")]
    public async Task<String> GetAsync(String appName = null,
                                       IPAddress clientIpAddress = null,
                                       String hostname = null,
                                       Guid? tenant = null,
                                       DateTime? dateTimeFrom = null,
                                       DateTime? dateTimeTo = null,
                                       String name = null,
                                       String spanId = null,
                                       String traceId = null,
                                       CancellationToken cancellationToken = default)
    {
        var eventDtos = await _eventsService.GetUsingFiltersAsync(appName, clientIpAddress, hostname, tenant, dateTimeFrom, dateTimeTo, name, spanId, traceId, cancellationToken)
                                            .ConfigureAwait(false);

        return Json.ToString(eventDtos);
    }
}
