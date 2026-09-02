using System;
using Zetatech.Accelerate.Logging.Abstractions;

namespace Zetatech.DeepSight.Logging;

public sealed class DeepSightLoggerOptions : BaseLoggerOptions
{
    public String AppName { get; set; }
    public Version AppVersion { get; set; }
    public Uri Uri { get; set; }
}
