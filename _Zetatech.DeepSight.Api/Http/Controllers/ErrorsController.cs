using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zetatech.Accelerate.Exceptions;
using Zetatech.Accelerate.Http.Abstractions;
using Zetatech.Accelerate.Http.Extensions;
using Zetatech.DeepSight.Application.Dtos;
using Zetatech.DeepSight.Application.Services;

namespace Zetatech.DeepSight.Http.Controllers;

[Route("/api/v1/{tenant:guid}/errors")]
public sealed class ErrorsController : BaseApiController
{
    private readonly IErrorsService _errorsService;
    private readonly ILogger _logger;

    public ErrorsController(ILoggerFactory loggerFactory,
                            IErrorsService errorsService)
    {
        _errorsService = errorsService ?? throw new ArgumentException("The provided errors service must be a valid instance", nameof(errorsService));
        _logger = loggerFactory.CreateLogger<ErrorsController>();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromRoute(Name = "tenant")] Guid tenantId)
    {
        if (!HttpContext.Request.HasJsonContentType())
        {
            throw new ValidationException("Request body is an invalid json object");
        }

        _logger.LogDebug($"Received a new error http request for tenant {tenantId}");
        _logger.LogDebug($"Reading the http request body");
        var deepSightDto = await HttpContext.Request.ReadBodyAsJsonAsync<DeepSightDto>()
                                                    .ConfigureAwait(false);

        var activity = Activity.Current;

        deepSightDto.ClientIpAddress = HttpContext.Connection.RemoteIpAddress;
        deepSightDto.TenantId = tenantId;
        deepSightDto.SpanId = activity?.ParentSpanId.ToString();
        deepSightDto.TraceId = activity?.TraceId.ToString();

        await _errorsService.PublishAsync(deepSightDto)
                            .ConfigureAwait(false);

        return Accepted();
    }
}
