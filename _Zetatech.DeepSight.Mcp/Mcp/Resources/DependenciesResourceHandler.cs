using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol.Server;
using Zetatech.Accelerate.Serialization;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class DependenciesResourceHandler
{
    private readonly IDependenciesService _dependenciesService;

    public DependenciesResourceHandler(IDependenciesService dependenciesService)
    {
        _dependenciesService = dependenciesService;
    }

    [McpServerResource(UriTemplate = "db://dependencies{?appName,clientIpAddress,hostname,tenant,dateTimeFrom,dateTimeTo,durationFrom,durationTo,name,spanId,success,target,traceId,type}", MimeType = "application/json")]
    public async Task<String> GetAsync(String appName = null,
                                       IPAddress clientIpAddress = null,
                                       String hostname = null,
                                       Guid? tenant = null,
                                       DateTime? dateTimeFrom = null,
                                       DateTime? dateTimeTo = null,
                                       Double? durationFrom = null,
                                       Double? durationTo = null,
                                       String name = null,
                                       String spanId = null,
                                       Boolean? success = null,
                                       String target = null,
                                       String traceId = null,
                                       String type = null,
                                       CancellationToken cancellationToken = default)
    {
        var dependencyDtos = await _dependenciesService.GetUsingFiltersAsync(appName, clientIpAddress, hostname, tenant, dateTimeFrom, dateTimeTo, durationFrom, durationTo, name, spanId, success, target, traceId, type, cancellationToken)
                                                       .ConfigureAwait(false);

        return Json.ToString(dependencyDtos);
    }
}
