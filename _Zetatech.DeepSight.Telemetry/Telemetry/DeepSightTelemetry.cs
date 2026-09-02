using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Zetatech.Accelerate.Serialization;
using Zetatech.Accelerate.Telemetry.Abstractions;

namespace Zetatech.DeepSight.Telemetry;

public sealed class DeepSightTelemetry : BaseTelemetry
{
    private HttpClient _httpClient;
    private Boolean _disposed;
    private readonly DeepSightTelemetryOptions _options;

    public DeepSightTelemetry(IOptions<DeepSightTelemetryOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentException("The provided configuration options must be a valid instance", nameof(options));
        _httpClient = new HttpClient
        {
            BaseAddress = _options.Uri,
            Timeout = TimeSpan.FromSeconds(1)
        };

        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (!$"{_httpClient.BaseAddress}".EndsWith("/"))
        {
            _httpClient.BaseAddress = new Uri($"{_httpClient.BaseAddress}/");
        }
    }

    protected override void Dispose(Boolean disposing)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }

        _disposed = true;

        base.Dispose(disposing);

        if (disposing)
        {
            _httpClient = null;
        }
    }
    private async Task SendTelemetryDataAsync(String urlRelativePath,
                                              IDictionary<String, Object> metadata,
                                              CancellationToken cancellationToken = default)
    {
        var activity = Activity.Current;
        var traceSpan = String.Empty;

        if (activity != null)
        {
            var traceflag = activity.ActivityTraceFlags == ActivityTraceFlags.Recorded ? "01" : "00";
            traceSpan = $"00-{activity.TraceId}-{activity.SpanId}-{traceflag}";
        }

        var clientAssembly = Assembly.GetExecutingAssembly().GetName();
        var deepSightTelemetryDto = new DeepSightTelemetryDto
        {
            AppName = _options.AppName,
            AppVersion = _options.AppVersion,
            ClientVersion = clientAssembly.Version,
            HostName = Environment.MachineName,
            Metadata = metadata,
            Timestamp = DateTime.UtcNow
        };

        try
        {
            var jsonBody = Json.ToString(deepSightTelemetryDto);

            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, urlRelativePath);

            httpRequestMessage.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            if (!String.IsNullOrEmpty(traceSpan))
            {
                httpRequestMessage.Headers.Add("tracespan", traceSpan);
            }

            await _httpClient.SendAsync(httpRequestMessage, cancellationToken)
                             .ConfigureAwait(false);
        }
        catch
        {
        }
    }
    public override async Task TrackDependencyAsync(String name,
                                                    String type,
                                                    String target,
                                                    Boolean success,
                                                    Double duration,
                                                    Byte[] dataInput = null,
                                                    Byte[] dataOutput = null,
                                                    IDictionary<String, Object> metadata = null,
                                                    CancellationToken cancellationToken = default)
    {
        var telemetryData = new Dictionary<String, Object>
        {
            { "dataInput", dataInput == null ? null : Convert.ToBase64String(dataInput) },
            { "dataOutput", dataOutput == null ? null : Convert.ToBase64String(dataOutput) },
            { "duration", duration },
            { "name", name },
            { "success", success },
            { "target", target },
            { "type", type }
        };

        await SendTelemetryDataAsync("dependencies", telemetryData, cancellationToken).ConfigureAwait(false);
    }
    public override async Task TrackEventAsync(String name,
                                               IDictionary<String, Object> metadata = null,
                                               CancellationToken cancellationToken = default)
    {
        var telemetryData = new Dictionary<String, Object>
        {
            { "name", name }
        };

        await SendTelemetryDataAsync("events", telemetryData, cancellationToken).ConfigureAwait(false);
    }
    public override async Task TrackMetricAsync(String name,
                                                String dimension,
                                                Double value,
                                                IDictionary<String, Object> metadata = null,
                                                CancellationToken cancellationToken = default)
    {
        var telemetryData = new Dictionary<String, Object>
        {
            { "dimension", dimension },
            { "name", name },
            { "value", value }
        };

        await SendTelemetryDataAsync("metrics", telemetryData, cancellationToken).ConfigureAwait(false);
    }
    public override async Task TrackPageViewAsync(String name,
                                                  String deviceType,
                                                  Uri uri = null,
                                                  String userAgent = null,
                                                  IDictionary<String, Object> metadata = null,
                                                  CancellationToken cancellationToken = default)
    {
        var telemetryData = new Dictionary<String, Object>
        {
            { "deviceType", deviceType },
            { "name", name },
            { "uri", uri?.ToString() },
            { "userAgent", userAgent }
        };

        await SendTelemetryDataAsync("pageviews", telemetryData, cancellationToken).ConfigureAwait(false);
    }
    public override async Task TrackRequestAsync(String name,
                                                 String endpoint,
                                                 String type,
                                                 Boolean success,
                                                 Double duration,
                                                 IPAddress ipAddress,
                                                 Int32 statusCode,
                                                 Byte[] dataInput = null,
                                                 Byte[] dataOutput = null,
                                                 IDictionary<String, Object> metadata = null,
                                                 CancellationToken cancellationToken = default)
    {
        var telemetryData = new Dictionary<String, Object>
        {
            { "dataInput", dataInput == null ? null : Convert.ToBase64String(dataInput) },
            { "dataOutput", dataOutput == null ? null : Convert.ToBase64String(dataOutput) },
            { "duration", duration },
            { "endpoint", endpoint },
            { "ipAddress", ipAddress?.ToString() },
            { "name", name },
            { "statusCode", statusCode },
            { "success", success },
            { "type", type }
        };

        await SendTelemetryDataAsync("requests", telemetryData, cancellationToken).ConfigureAwait(false);
    }
    public override async Task TrackTestResultAsync(String name,
                                                    Boolean success,
                                                    Double duration,
                                                    String message = null,
                                                    IDictionary<String, Object> metadata = null,
                                                    CancellationToken cancellationToken = default)
    {
        var telemetryData = new Dictionary<String, Object>
        {
            { "duration", duration },
            { "message", message },
            { "name", name },
            { "success", success }
        };

        await SendTelemetryDataAsync("tests", telemetryData, cancellationToken).ConfigureAwait(false);
    }
}
