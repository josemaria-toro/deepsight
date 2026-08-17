using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.AspNetCore;
using Zetatech.Accelerate.DependencyInjection;
using Zetatech.DeepSight.DependencyInjection;
using Zetatech.DeepSight.Mcp.Resources;

namespace Zetatech.DeepSight;

public class Program
{
     public static async Task Main(String[] argv)
     {
          var appBuilder = WebApplication.CreateBuilder();

          appBuilder.Configuration.AddConfigurationSources();
          appBuilder.Logging.ClearProviders();
          appBuilder.Services.AddApplicationServices()
                             .AddDeepSightLoggerProvider()
                             .AddDeepSightLoggerProviderOptions()
                             .AddDeepSightTelemetry()
                             .AddDeepSightTelemetryOptions()
                             .AddDomainRepositories()
                             .AddDomainRepositoriesOptions()
                             .AddMcpServer(options =>
                             {
                                  options.Capabilities = new ModelContextProtocol.Protocol.ServerCapabilities
                                  {
                                       Resources = new ModelContextProtocol.Protocol.ResourcesCapability
                                       {
                                            ListChanged = true,
                                            Subscribe = true
                                       }
                                  };
                             })
                             .WithHttpTransport(o => o.SessionMode = HttpServerSessionMode.Stateless)
                             .WithResourcesFromAssembly();

          var app = appBuilder.Build();

          app.MapMcp("api/v1/mcp");

          await app.StartAsync()
                   .ConfigureAwait(false);
          await app.WaitForShutdownAsync()
                   .ConfigureAwait(false);
     }
}