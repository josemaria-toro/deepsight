using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zetatech.DeepSight.Logging;
using Zetatech.DeepSight.Logging.Providers;

namespace Zetatech.DeepSight.DependencyInjection;

public static partial class DependencyInjection
{
    public static IServiceCollection AddDeepSightLoggerProvider(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<ILoggerProvider, DeepSightLoggerProvider>();
    }
    public static IServiceCollection AddDeepSightLoggerProviderOptions(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOptions<DeepSightLoggerOptions>()
                         .Configure<IConfiguration>((options, configService) =>
                         {
                             options.AppName = configService.GetValue<String>("logging:deepSight:appName", String.Empty);
                             options.AppVersion = configService.GetValue<Version>("logging:deepSight:appVersion", Version.Parse("1.0.0"));
                             options.LogLevel = configService.GetValue<LogLevel>("logging:logLevel:deepSight", LogLevel.Warning);
                             options.Uri = configService.GetValue<Uri>("logging:deepSight:url");
                         });

        return serviceCollection;
    }
}
