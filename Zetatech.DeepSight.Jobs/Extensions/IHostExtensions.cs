using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zetatech.DeepSight.Jobs;

namespace Zetatech.DeepSight.Extensions;

public static partial class IHostExtensions
{
    public static async Task StartJobsAsync(this IHost appHost)
    {
        await appHost.Services.GetRequiredService<RemoveObsoleteDependenciesJob>()
                              .StartAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoleteErrorsJob>()
                              .StartAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoleteEventsJob>()
                              .StartAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoleteMetricsJob>()
                              .StartAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoletePageViewsJob>()
                              .StartAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoleteRequestsJob>()
                              .StartAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoleteTestsJob>()
                              .StartAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoleteTracesJob>()
                              .StartAsync(default)
                              .ConfigureAwait(false);
    }
    public static async Task StopJobsAsync(this IHost appHost)
    {
        await appHost.Services.GetRequiredService<RemoveObsoleteDependenciesJob>()
                              .StopAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoleteErrorsJob>()
                              .StopAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoleteEventsJob>()
                              .StopAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoleteMetricsJob>()
                              .StopAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoletePageViewsJob>()
                              .StopAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoleteRequestsJob>()
                              .StopAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoleteTestsJob>()
                              .StopAsync(default)
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<RemoveObsoleteTracesJob>()
                              .StopAsync(default)
                              .ConfigureAwait(false);
    }
}
