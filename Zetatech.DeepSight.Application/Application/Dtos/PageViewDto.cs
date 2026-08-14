using System;
using Zetatech.DeepSight.Application.Abstractions;

namespace Zetatech.DeepSight.Application.Dtos;

public sealed record PageViewDto : BaseDeepSightDto
{
    public String DeviceType { get; set; }
    public String Name { get; set; }
    public Uri Url { get; set; }
    public String UserAgent { get; set; }
}