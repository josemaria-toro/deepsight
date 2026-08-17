using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Resources;

[McpServerResourceType]
public class TestsResourceHandler
{
    private readonly ILogger _logger;
    private readonly ITestsService _testsService;

    public TestsResourceHandler(ITestsService testsService,
                                ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TestsResourceHandler>();
        _testsService = testsService;
    }

    [McpServerResource(UriTemplate = "db://tests")]
    public async Task<String> GetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Se obtienen las pruebas");
        return "Estas son tus pruebas";
    }
}