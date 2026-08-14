using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Zetatech.Accelerate.DependencyInjection;
using Zetatech.Accelerate.Http.Middlewares;
using Zetatech.DeepSight.DependencyInjection;
using Zetatech.DeepSight.Http.Middlewares;

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
           .UseMiddleware<LogHttpRequestMiddleware>()
           .UseMiddleware<SecurityHeadersMiddleware>()
           .UseMiddleware<ExceptionsHandlerMiddleware>()
           .UseMiddleware<CheckTenantInRouteMiddleware>();
        app.UseMvcFeatures();
        app.UseRateLimitsFeatures();

        await app.StartAsync();
        await app.WaitForShutdownAsync();
    }
}