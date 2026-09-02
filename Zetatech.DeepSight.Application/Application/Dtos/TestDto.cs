using System;
using Zetatech.DeepSight.Application.Abstractions;

namespace Zetatech.DeepSight.Application.Dtos;

public sealed record TestDto : BaseDeepSightDto
{
    public Double? Duration { get; set; }
    public String Message { get; set; }
    public String Name { get; set; }
    public Boolean? Success { get; set; }
}
