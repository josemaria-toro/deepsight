using Microsoft.Extensions.DependencyInjection;
using Zetatech.DeepSight.Application.Services;
using Zetatech.DeepSight.Infrastructure.Services;

namespace Zetatech.DeepSight.DependencyInjection;

public static partial class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<IDependenciesService, DependenciesService>()
                                .AddTransient<IErrorsService, ErrorsService>()
                                .AddTransient<IEventsService, EventsService>()
                                .AddTransient<IMetricsService, MetricsService>()
                                .AddTransient<IPageViewsService, PageViewsService>()
                                .AddTransient<IRequestsService, RequestsService>()
                                .AddTransient<ITestsService, TestsService>()
                                .AddTransient<ITracesService, TracesService>();
    }
}