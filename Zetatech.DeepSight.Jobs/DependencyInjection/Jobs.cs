using Microsoft.Extensions.DependencyInjection;
using Zetatech.DeepSight.Jobs;

namespace Zetatech.DeepSight.DependencyInjection;

public static partial class DependencyInjection
{
    public static IServiceCollection AddPeriodicJobs(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<RemoveObsoleteDependenciesJob>()
                                .AddTransient<RemoveObsoleteErrorsJob>()
                                .AddTransient<RemoveObsoleteEventsJob>()
                                .AddTransient<RemoveObsoleteMetricsJob>()
                                .AddTransient<RemoveObsoletePageViewsJob>()
                                .AddTransient<RemoveObsoleteRequestsJob>()
                                .AddTransient<RemoveObsoleteTestsJob>()
                                .AddTransient<RemoveObsoleteTracesJob>();
    }
}
