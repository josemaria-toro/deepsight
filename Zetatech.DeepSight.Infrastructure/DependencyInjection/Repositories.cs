using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zetatech.Accelerate.Data;
using Zetatech.Accelerate.Data.Enums;
using Zetatech.DeepSight.Domain.Repositories;
using Zetatech.DeepSight.Infrastructure.Persistency;

namespace Zetatech.DeepSight.DependencyInjection;

public static partial class DependencyInjection
{
    public static IServiceCollection AddDomainRepositories(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<IDependenciesRepository, DependenciesRepository>()
                                .AddTransient<IErrorsRepository, ErrorsRepository>()
                                .AddTransient<IEventsRepository, EventsRepository>()
                                .AddTransient<IMetricsRepository, MetricsRepository>()
                                .AddTransient<IPageViewsRepository, PageViewsRepository>()
                                .AddTransient<IRequestsRepository, RequestsRepository>()
                                .AddTransient<ITestsRepository, TestsRepository>()
                                .AddTransient<ITracesRepository, TracesRepository>();
    }
    public static IServiceCollection AddDomainRepositoriesOptions(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOptions<EntityFrameworkRepositoryOptions>()
                         .Configure<IConfiguration>((options, configService) =>
                         {
                             options.ConnectionString = configService.GetConnectionString("database");
                             options.Engine = configService.GetValue<DatabaseEngines>("database:engine", DatabaseEngines.PostgreSql);
                             options.Timeout = configService.GetValue<Int32>("database:timeout", 30);
                         });

        return serviceCollection;
    }
}