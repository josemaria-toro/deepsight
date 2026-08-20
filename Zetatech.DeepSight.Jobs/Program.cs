using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Zetatech.Accelerate.DependencyInjection;
using Zetatech.DeepSight.DependencyInjection;
using Zetatech.DeepSight.Extensions;

namespace Zetatech.DeepSight;

public class Program
{
    public static async Task Main(String[] argv)
    {
        var appBuilder = Host.CreateApplicationBuilder(argv);

        appBuilder.Configuration.AddConfigurationSources();
        appBuilder.Logging.ClearProviders();
        appBuilder.Services.AddApplicationServices()
                           .AddConsoleLoggerProvider()
                           .AddConsoleLoggerProviderOptions()
                           .AddDeepSightLoggerProvider()
                           .AddDeepSightLoggerProviderOptions()
                           .AddDomainRepositories()
                           .AddDomainRepositoriesOptions()
                           .AddJobs();

        var app = appBuilder.Build();

        await app.StartAsync()
                 .ConfigureAwait(false);
        await app.StartJobsAsync()
                 .ConfigureAwait(false);
        await app.WaitForShutdownAsync()
                 .ConfigureAwait(false);
        await app.StopJobsAsync()
                 .ConfigureAwait(false);
    }
}
