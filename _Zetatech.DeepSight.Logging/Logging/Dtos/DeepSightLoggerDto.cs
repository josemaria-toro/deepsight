using System;
using System.Collections.Generic;

namespace Zetatech.DeepSight.Logging.Dtos;

internal record DeepSightLoggerDto
{
    public String AppName { get; set; }
    public Version AppVersion { get; set; }
    public Version ClientVersion { get; set; }
    public String HostName { get; set; }
    public IDictionary<String, Object> Metadata { get; set; }
    public DateTime Timestamp { get; set; }
}
