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
public class ErrorsResourceHandler
{
    private readonly IErrorsService _errorsService;

    public ErrorsResourceHandler(IErrorsService errorsService)
    {
        _errorsService = errorsService;
    }

    [McpServerResource(UriTemplate = "db://errors{?appName,clientIpAddress,hostname,tenant,dateTimeFrom,dateTimeTo,category,message,severity,spanId,traceId,type}", MimeType = "application/json")]
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
                                       String type = null,
                                       CancellationToken cancellationToken = default)
    {
        var errorDtos = await _errorsService.GetUsingFiltersAsync(appName, clientIpAddress, hostname, tenant, dateTimeFrom, dateTimeTo, category, message, severity, spanId, traceId, type, cancellationToken)
                                            .ConfigureAwait(false);

        return Json.ToString(errorDtos);
    }
}
