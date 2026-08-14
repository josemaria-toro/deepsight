using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zetatech.Accelerate.Logging.Abstractions;
using Zetatech.DeepSight.Logging.Dtos;
using Zetatech.Accelerate.Serialization;

namespace Zetatech.DeepSight.Logging;

public sealed class DeepSightLogger : BaseLogger<DeepSightLoggerOptions>
{
    private readonly HttpClient _httpClient;

    public DeepSightLogger(IOptions<DeepSightLoggerOptions> options,
                           String category,
                           HttpClient httpClient) : base(options, category)
    {
        _httpClient = httpClient ?? throw new ArgumentException("The provided http client must be a valid instance", nameof(httpClient));
    }

    public override void Log<TState>(LogLevel logLevel,
                                     EventId eventId,
                                     TState state,
                                     Exception exception,
                                     Func<TState, Exception, String> formatter)
    {
        if (IsEnabled(logLevel))
        {
            var activity = Activity.Current;
            var traceSpan = String.Empty;

            if (activity != null)
            {
                var traceflag = activity.ActivityTraceFlags == ActivityTraceFlags.Recorded ? "01" : "00";
                traceSpan = $"00-{activity.TraceId}-{activity.SpanId}-{traceflag}";
            }

            var clientAssembly = Assembly.GetExecutingAssembly().GetName();
            var loggerDto = new DeepSightLoggerDto
            {
                AppName = Options.AppName,
                AppVersion = Options.AppVersion,
                ClientVersion = clientAssembly.Version,
                HostName = Environment.MachineName,
                Metadata = new Dictionary<String, Object>(),
                Timestamp = DateTime.UtcNow
            };

            TrackException(logLevel, exception, loggerDto, traceSpan);
            TrackTrace(logLevel, $"{state}", loggerDto, traceSpan);
        }
        }
    private void TrackException(LogLevel logLevel,
                                Exception exception,
                                DeepSightLoggerDto deepSightLoggerDto,
                                String traceSpan)
    {
        while (exception != null)
        {
            deepSightLoggerDto.Metadata.Clear();
            deepSightLoggerDto.Metadata.Add("category", Category);
            deepSightLoggerDto.Metadata.Add("message", exception.Message);
            deepSightLoggerDto.Metadata.Add("severity", logLevel);
            deepSightLoggerDto.Metadata.Add("stackTrace", exception.StackTrace);
            deepSightLoggerDto.Metadata.Add("typeName", exception.GetType().Name);

            try
            {
                var jsonBody = Json.ToString(deepSightLoggerDto);

                using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "errors");

                httpRequestMessage.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                if (!String.IsNullOrEmpty(traceSpan))
                {
                    httpRequestMessage.Headers.Add("tracespan", traceSpan);
                }

                _httpClient.SendAsync(httpRequestMessage).Wait(250);
            }
            catch
            {
            }
            finally
            {
                exception = exception.InnerException;
            }
        }
    }
    private void TrackTrace(LogLevel logLevel,
                            String message,
                            DeepSightLoggerDto deepSightLoggerDto,
                            String traceSpan)
    {
        deepSightLoggerDto.Metadata.Clear();
        deepSightLoggerDto.Metadata.Add("category", Category);
        deepSightLoggerDto.Metadata.Add("message", message);
        deepSightLoggerDto.Metadata.Add("severity", logLevel);

        try
        {
            var jsonBody = Json.ToString(deepSightLoggerDto);

            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "traces");

            httpRequestMessage.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            if (!String.IsNullOrEmpty(traceSpan))
            {
                httpRequestMessage.Headers.Add("tracespan", traceSpan);
            }

            _httpClient.SendAsync(httpRequestMessage).Wait(250);
        }
        catch
        {
        }
    }
}
