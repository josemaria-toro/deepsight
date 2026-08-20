using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol.Server;
using Zetatech.Accelerate.Serialization;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class RequestsResourceHandler
{
    private readonly IRequestsService _requestsService;

    public RequestsResourceHandler(IRequestsService requestsService)
    {
        _requestsService = requestsService;
    }

    [McpServerResource(UriTemplate = "db://requests{?appName,clientIpAddress,hostname,tenant,dateTimeFrom,dateTimeTo,durationFrom,durationTo,endpoint,ipAddress,name,spanId,statusCode,success,traceId,type}", MimeType = "application/json")]
    public async Task<String> GetAsync(String appName = null,
                                       IPAddress clientIpAddress = null,
                                       String hostname = null,
                                       Guid? tenant = null,
                                       DateTime? dateTimeFrom = null,
                                       DateTime? dateTimeTo = null,
                                       Double? durationFrom = null,
                                       Double? durationTo = null,
                                       String endpoint = null,
                                       IPAddress ipAddress = null,
                                       String name = null,
                                       String spanId = null,
                                       Int32? statusCode = null,
                                       Boolean? success = null,
                                       String traceId = null,
                                       String type = null,
                                       CancellationToken cancellationToken = default)
    {
        var requestDtos = await _requestsService.GetUsingFiltersAsync(appName, clientIpAddress, hostname, tenant, dateTimeFrom, dateTimeTo, durationFrom, durationTo, endpoint, ipAddress, name, spanId, statusCode, success, traceId, type, cancellationToken)
                                                .ConfigureAwait(false);

        return Json.ToString(requestDtos);
    }
}
