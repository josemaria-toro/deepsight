using System;
using System.Collections.Generic;
using System.Net;
using Zetatech.Accelerate.Application.Abstractions;

namespace Zetatech.DeepSight.Application.Abstractions;

public abstract record BaseDeepSightDto : BaseDto
{
    public String AppName { get; set; }
    public Version AppVersion { get; set; }
    public IPAddress ClientIpAddress { get; set; }
    public Version ClientVersion { get; set; }
    public String HostName { get; set; }
    public IDictionary<String, Object> Metadata { get; set; }
    public String SpanId { get; set; }
    public Guid TenantId { get; set; }
    public DateTime Timestamp { get; set; }
    public String TraceId { get; set; }
}
