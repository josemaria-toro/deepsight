using System;
using Zetatech.Accelerate.Application.Abstractions;

namespace Zetatech.DeepSight.Application.Abstractions;

public abstract record BaseDeepSightDto : BaseDto
{
    public String AppName { get; set; }
    public String AppVersion { get; set; }
    public String ClientIpAddress { get; set; }
    public String ClientVersion { get; set; }
    public String HostName { get; set; }
    public String Metadata { get; set; }
    public String SpanId { get; set; }
    public Guid? TenantId { get; set; }
    public DateTime? Timestamp { get; set; }
    public String TraceId { get; set; }
}
