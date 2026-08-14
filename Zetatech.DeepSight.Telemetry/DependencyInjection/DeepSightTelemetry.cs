// using System;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Zetatech.Accelerate.Telemetry;
// using Zetatech.DeepSight.Telemetry;

// namespace Zetatech.DeepSight.DependencyInjection;

// public static partial class DependencyInjection
// {
//     public static IServiceCollection AddDeepSightTelemetry(this IServiceCollection serviceCollection)
//     {
//         return serviceCollection.AddSingleton<ITelemetry, DeepSightTelemetry>();
//     }
//     public static IServiceCollection AddDeepSightTelemetryOptions(this IServiceCollection serviceCollection)
//     {
//         serviceCollection.AddOptions<DeepSightTelemetryOptions>()
//                          .Configure<IConfiguration>((options, configService) =>
//                          {
//                              options.AppName = configService.GetValue<String>("telemetry:deepSight:appName", String.Empty);
//                              options.AppVersion = configService.GetValue<Version>("telemetry:deepSight:appVersion", Version.Parse("1.0.0"));
//                              options.Uri = configService.GetValue<Uri>("telemetry:deepSight:url");
//                          });

//         return serviceCollection;
//     }
// }