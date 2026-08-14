using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Zetatech.Accelerate.Messaging;
using Zetatech.DeepSight.Application.Services;
using Zetatech.DeepSight.Application.Subscribers;
using Zetatech.DeepSight.Infrastructure.Subscribers;

namespace Zetatech.DeepSight.DependencyInjection;

public static partial class DependencyInjection
{
    public static IServiceCollection AddMessageSubscribers(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<IDependenciesSubscriber, DependenciesSubscriber, IDependenciesService>("dependencies")
                                .AddTransient<IErrorsSubscriber, ErrorsSubscriber, IErrorsService>("errors")
                                .AddTransient<IEventsSubscriber, EventsSubscriber, IEventsService>("events")
                                .AddTransient<IMetricsSubscriber, MetricsSubscriber, IMetricsService>("metrics")
                                .AddTransient<IPageViewsSubscriber, PageViewsSubscriber, IPageViewsService>("pageviews")
                                .AddTransient<IRequestsSubscriber, RequestsSubscriber, IRequestsService>("requests")
                                .AddTransient<ITestsSubscriber, TestsSubscriber, ITestsService>("tests")
                                .AddTransient<ITracesSubscriber, TracesSubscriber, ITracesService>("traces");
    }

    private static IServiceCollection AddTransient<TSubscriber, TSubscriberInstance, TApplicationService>(this IServiceCollection serviceCollection, String queueName) where TSubscriber : class
                                                                                                                                                                       where TSubscriberInstance : class, TSubscriber
    {
        return serviceCollection.AddTransient<TSubscriber, TSubscriberInstance>(serviceProvider =>
        {
            var applicationService = serviceProvider.GetRequiredService<TApplicationService>();
            var channelFactory = serviceProvider.GetRequiredService<IRabbitMQChannelFactory>();
            var configService = serviceProvider.GetRequiredService<IConfiguration>();
            var subscriberOptions = new RabbitMQOptions
            {
                ConnectionString = configService.GetConnectionString("messageBroker"),
                QueueName = queueName,
                SslCertIssuer = configService.GetValue<String>("messageBroker:issuer", String.Empty),
                SslCertSerialNumber = configService.GetValue<String>("messageBroker:serialNumber", String.Empty),
                SslCertSubject = configService.GetValue<String>("messageBroker:subject", String.Empty),
                SslCertThumbprint = configService.GetValue<String>("messageBroker:thumbprint", String.Empty),
                UseSsl = configService.GetValue<Boolean>("messageBroker:useSsl", false)
            };

            return (TSubscriberInstance)Activator.CreateInstance(typeof(TSubscriberInstance),
                                                                 Options.Create(subscriberOptions),
                                                                 channelFactory,
                                                                 applicationService);
        });
    }
}