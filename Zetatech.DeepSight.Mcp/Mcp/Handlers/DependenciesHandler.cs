using System;
using System.Threading.Tasks;
using ModelContextProtocol.Server;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Mcp.Handlers;

[McpServerResourceType]
public class DependenciesHandler
{
    private readonly IDependenciesService _dependenciesService;

    public DependenciesHandler(IDependenciesService dependenciesService)
    {
        _dependenciesService = dependenciesService;
    }

    [McpServerResource(UriTemplate = "db://dependencies")]
    public async Task<string> GetAsync()
    {
        return "Hola que haces?";
    }
}