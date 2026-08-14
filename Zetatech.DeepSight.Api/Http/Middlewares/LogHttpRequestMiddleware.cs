using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Zetatech.DeepSight.Http.Middlewares;

public sealed class LogHttpRequestMiddleware
{
    private readonly RequestDelegate _next;

    public LogHttpRequestMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentException("The provided http context must be a valid instance", nameof(httpContext));
        }

        var utcnow = DateTime.UtcNow;
        var logger = httpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                                                .CreateLogger<LogHttpRequestMiddleware>();

        logger.LogInformation($"{httpContext.Request.GetDisplayUrl()}");
        logger.LogInformation($"Client ip address: {httpContext.Connection.RemoteIpAddress}");
        logger.LogInformation($"Client port: {httpContext.Connection.RemotePort}");
        logger.LogInformation($"Http request headers:");

        foreach (var header in httpContext.Request.Headers)
        {
            logger.LogInformation($"- {header.Key}: {header.Value}");
        }

        httpContext.Response.OnCompleted(async () =>
        {
            var duration = (DateTime.UtcNow - utcnow).TotalMilliseconds;

            logger.LogInformation($"Response status code: {httpContext.Response.StatusCode}");
            logger.LogInformation($"Duration: {duration:0.00000}");
            logger.LogInformation($"Http response headers:");

            foreach (var header in httpContext.Response.Headers)
            {
                logger.LogInformation($"- {header.Key}: {header.Value}");
            }
        });

        await _next(httpContext);
    }
}