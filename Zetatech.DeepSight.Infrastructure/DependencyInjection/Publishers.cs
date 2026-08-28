using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zetatech.Accelerate.Messaging;
using Zetatech.DeepSight.Domain.Publishers;
using Zetatech.DeepSight.Infrastructure.Publishers;

namespace Zetatech.DeepSight.DependencyInjection;

public static partial class DependencyInjection
{
    public static IServiceCollection AddMessagePublishers(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IDeepSightPublisher, DeepSightPublisher>();
    }
    public static IServiceCollection AddMessagePublishersOptions(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOptions<RabbitMQOptions>()
                         .Configure<IConfiguration>((options, configService) =>
                         {
                             options.ConnectionString = configService.GetConnectionString("messageBroker");
                             options.ExchangeName = configService.GetValue<String>("messageBroker:exchange", "amq.direct");
                             options.SslCertIssuer = configService.GetValue<String>("messageBroker:issuer", String.Empty);
                             options.SslCertSerialNumber = configService.GetValue<String>("messageBroker:serialNumber", String.Empty);
                             options.SslCertSubject = configService.GetValue<String>("messageBroker:subject", String.Empty);
                             options.SslCertThumbprint = configService.GetValue<String>("messageBroker:thumbprint", String.Empty);
                             options.UseSsl = configService.GetValue<Boolean>("messageBroker:useSsl", false);
                         });

        return serviceCollection;
    }
}
