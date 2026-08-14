using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zetatech.DeepSight.Application.Subscribers;

namespace Zetatech.DeepSight.Extensions;

public static partial class IHostExtensions
{
    public static async Task SubscribeAsync(this IHost appHost)
    {
        await appHost.Services.GetRequiredService<IDependenciesSubscriber>()
                              .SubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<IErrorsSubscriber>()
                              .SubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<IEventsSubscriber>()
                              .SubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<IMetricsSubscriber>()
                              .SubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<IPageViewsSubscriber>()
                              .SubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<IRequestsSubscriber>()
                              .SubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<ITestsSubscriber>()
                              .SubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<ITracesSubscriber>()
                              .SubscribeAsync()
                              .ConfigureAwait(false);
    }
    public static async Task UnsubscribeAsync(this IHost appHost)
    {
        await appHost.Services.GetRequiredService<IDependenciesSubscriber>()
                              .UnsubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<IErrorsSubscriber>()
                              .UnsubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<IEventsSubscriber>()
                              .UnsubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<IMetricsSubscriber>()
                              .UnsubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<IPageViewsSubscriber>()
                              .UnsubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<IRequestsSubscriber>()
                              .UnsubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<ITestsSubscriber>()
                              .UnsubscribeAsync()
                              .ConfigureAwait(false);
        await appHost.Services.GetRequiredService<ITracesSubscriber>()
                              .UnsubscribeAsync()
                              .ConfigureAwait(false);
    }
}