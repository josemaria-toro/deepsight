using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zetatech.Accelerate.Exceptions;

namespace Zetatech.DeepSight.Http.Middlewares;

public sealed class CheckTenantInRouteMiddleware
{
    private readonly RequestDelegate _next;

    public CheckTenantInRouteMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentException("The provided http context must be a valid instance", nameof(httpContext));
        }

        if (!httpContext.Request.RouteValues.ContainsKey("tenant"))
        {
            throw new NotFoundException("No tenant was found in the url");
        }

        var configService = httpContext.RequestServices.GetRequiredService<IConfiguration>();
        var allowedTenant = configService.GetValue<Guid>("appSettings:tenant", Guid.Empty);

        if (!Guid.TryParse(httpContext.Request.RouteValues["tenant"].ToString(), out var currentTenant))
        {
            throw new NotFoundException("The provided tenant has an invalid format");
        }

        if (allowedTenant != currentTenant)
        {
            throw new ValidationException("The provided tenant is invalid");
        }

        await _next(httpContext);
    }
}