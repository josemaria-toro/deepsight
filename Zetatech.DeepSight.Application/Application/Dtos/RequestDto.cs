using System;
using Zetatech.DeepSight.Application.Abstractions;

namespace Zetatech.DeepSight.Application.Dtos;

public sealed record RequestDto : BaseDeepSightDto
{
    public String DataInput { get; set; }
    public String DataOutput { get; set; }
    public Double Duration { get; set; }
    public String EndPoint { get; set; }
    public String IPAddress { get; set; }
    public String Name { get; set; }
    public Int32? StatusCode { get; set; }
    public Boolean? Success { get; set; }
    public String Type { get; set; }
}
