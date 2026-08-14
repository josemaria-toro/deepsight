using System;
using Zetatech.DeepSight.Application.Abstractions;

namespace Zetatech.DeepSight.Application.Dtos;

public sealed record EventDto : BaseDeepSightDto
{
    public String Name { get; set; }
}