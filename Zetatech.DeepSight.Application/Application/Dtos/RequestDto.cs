using System;
using System.Net;
using Zetatech.DeepSight.Application.Abstractions;

namespace Zetatech.DeepSight.Application.Dtos;

public sealed record RequestDto : BaseDeepSightDto
{
    public Byte[] DataInput { get; set; }
    public Byte[] DataOutput { get; set; }
    public Double Duration { get; set; }
    public String EndPoint { get; set; }
    public IPAddress IPAddress { get; set; }
    public String Name { get; set; }
    public Int32? StatusCode { get; set; }
    public Boolean Success { get; set; }
    public String Type { get; set; }
}