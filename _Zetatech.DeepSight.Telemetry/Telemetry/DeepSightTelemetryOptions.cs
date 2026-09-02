using System;

namespace Zetatech.DeepSight.Telemetry;

public sealed class DeepSightTelemetryOptions
{
    public String AppName { get; set; }
    public Version AppVersion { get; set; }
    public Uri Uri { get; set; }
}
