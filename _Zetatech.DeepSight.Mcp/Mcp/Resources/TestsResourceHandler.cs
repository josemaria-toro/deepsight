using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol.Server;
using Zetatech.Accelerate.Serialization;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class TestsResourceHandler
{
    private readonly ITestsService _testsService;

    public TestsResourceHandler(ITestsService testsService)
    {
        _testsService = testsService;
    }

    [McpServerResource(UriTemplate = "db://tests{?appName,clientIpAddress,hostname,tenant,dateTimeFrom,dateTimeTo,durationFrom,durationTo,message,name,spanId,success,traceId}", MimeType = "application/json")]
    public async Task<String> GetAsync(String appName = null,
                                       IPAddress clientIpAddress = null,
                                       String hostname = null,
                                       Guid? tenant = null,
                                       DateTime? dateTimeFrom = null,
                                       DateTime? dateTimeTo = null,
                                       Double? durationFrom = null,
                                       Double? durationTo = null,
                                       String message = null,
                                       String name = null,
                                       String spanId = null,
                                       Boolean? success = null,
                                       String traceId = null,
                                       CancellationToken cancellationToken = default)
    {
        var testDtos = await _testsService.GetUsingFiltersAsync(appName, clientIpAddress, hostname, tenant, dateTimeFrom, dateTimeTo, durationFrom, durationTo, message, name, spanId, success, traceId, cancellationToken)
                                          .ConfigureAwait(false);

        return Json.ToString(testDtos);
    }
}
