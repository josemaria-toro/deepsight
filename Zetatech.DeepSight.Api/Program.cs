using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Zetatech.Accelerate.DependencyInjection;
using Zetatech.Accelerate.Http.Middlewares;
using Zetatech.DeepSight.DependencyInjection;

namespace Zetatech.DeepSight;

public class Program
{
    public static async Task Main(String[] argv)
    {
        var appBuilder = WebApplication.CreateBuilder(argv);

        appBuilder.Configuration.AddConfigurationSources();
        appBuilder.Logging.ClearProviders();
        appBuilder.Services.AddApplicationServices()
                           .AddConsoleLoggerProvider()
                           .AddConsoleLoggerProviderOptions()
                           .AddCorsPolicies()
                           .AddMessagePublishers()
                           .AddMessagePublishersOptions()
                           .AddMvcComponents()
                           .AddRabbitMQChannelFactory()
                           .AddRateLimitsPolicies();

        var app = appBuilder.Build();

        app.UseCorsFeatures();
        app.UseMiddleware<W3CActivityMiddleware>()
           .UseMiddleware<SecurityHeadersMiddleware>()
           .UseMiddleware<ExceptionsHandlerMiddleware>();
        app.UseMvcFeatures();
        app.UseRateLimitsFeatures();

        await app.StartAsync()
                 .ConfigureAwait(false);
        await app.WaitForShutdownAsync()
                 .ConfigureAwait(false);
    }
}
