using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class ErrorsResourceHandler
{
    private readonly IErrorsService _errorsService;
    private readonly ILogger _logger;

    public ErrorsResourceHandler(IErrorsService errorsService,
                                 ILoggerFactory loggerFactory)
    {
        _errorsService = errorsService;
        _logger = loggerFactory.CreateLogger<ErrorsResourceHandler>();
    }

    [McpServerResource(UriTemplate = "db://errors")]
    public async Task<String> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Se obtienen los errores");
        return "Estos son tus errores";
    }
}