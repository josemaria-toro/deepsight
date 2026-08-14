using System;
using Microsoft.Extensions.Logging;
using Zetatech.DeepSight.Application.Abstractions;

namespace Zetatech.DeepSight.Application.Dtos;

public sealed record TraceDto : BaseDeepSightDto
{
    public String Category { get; set; }
    public String Message { get; set; }
    public LogLevel Severity { get; set; }
}