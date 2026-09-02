using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using Zetatech.Accelerate.DependencyInjection;
using Zetatech.Accelerate.Http.Middlewares;
using Zetatech.DeepSight.DependencyInjection;

namespace Zetatech.DeepSight;

public class Program
{
     public static async Task Main(String[] argv)
     {
          var appBuilder = WebApplication.CreateBuilder();

          appBuilder.Configuration.AddConfigurationSources();
          appBuilder.Logging.ClearProviders();
          appBuilder.Services.AddApplicationServices()
                             .AddConsoleLoggerProvider()
                             .AddConsoleLoggerProviderOptions()
                             .AddDeepSightLoggerProvider()
                             .AddDeepSightLoggerProviderOptions()
                             .AddDeepSightTelemetry()
                             .AddDeepSightTelemetryOptions()
                             .AddDomainRepositories()
                             .AddDomainRepositoriesOptions()
                             .AddMcpServer(options =>
                             {
                                  options.Capabilities = new ServerCapabilities
                                  {
                                       Resources = new ResourcesCapability
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
          app.UseMiddleware<W3CActivityMiddleware>()
             .UseMiddleware<TrackRequestMiddleware>()
             .UseMiddleware<SecurityHeadersMiddleware>();

          await app.StartAsync()
                   .ConfigureAwait(false);
          await app.WaitForShutdownAsync()
                   .ConfigureAwait(false);
     }
}
