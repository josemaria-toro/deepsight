using System;
using Zetatech.DeepSight.Application.Abstractions;

namespace Zetatech.DeepSight.Application.Dtos;

public sealed record MetricDto : BaseDeepSightDto
{
    public String Dimension { get; set; }
    public String Name { get; set; }
    public Double? Value { get; set; }
}
