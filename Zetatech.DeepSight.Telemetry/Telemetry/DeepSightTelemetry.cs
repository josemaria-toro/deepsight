// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Net;
// using System.Net.Http;
// using System.Net.Http.Headers;
// using System.Reflection;
// using System.Text;
// using Microsoft.Extensions.Options;
// using Zetatech.Accelerate.Serialization;
// using Zetatech.Accelerate.Telemetry.Abstractions;

// namespace Zetatech.DeepSight.Telemetry;

// internal sealed class DeepSightTelemetry : BaseTelemetry
// {
//     private HttpClient _httpClient;
//     private Boolean _disposed;
//     private readonly DeepSightTelemetryOptions _options;

//     public DeepSightTelemetry(IOptions<DeepSightTelemetryOptions> options)
//     {
//         _options = options?.Value ?? throw new ArgumentException("The provided configuration options must be a valid instance", nameof(options));
//         _httpClient = new HttpClient { BaseAddress = _options.Uri };
//         _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//     }

//     protected override void Dispose(Boolean disposing)
//     {
//         if (_disposed)
//         {
//             throw new ObjectDisposedException(GetType().Name);
//         }

//         _disposed = true;

//         base.Dispose(disposing);

//         if (disposing)
//         {
//             _httpClient = null;
//         }
//     }
//     private void SendTelemetryData(String urlPath,
//                                    IDictionary<String, Object> metadata)
//     {
//         var activity = Activity.Current;
//         var traceSpan = String.Empty;

//         if (activity != null)
//         {
//             var traceflag = activity.ActivityTraceFlags == ActivityTraceFlags.Recorded ? "01" : "00";
//             traceSpan = $"00-{activity.TraceId}-{activity.SpanId}-{traceflag}";
//         }

//         var clientAssembly = Assembly.GetExecutingAssembly().GetName();
//         var deepSightTelemetryDto = new DeepSightTelemetryDto
//         {
//             AppName = _options.AppName,
//             AppVersion = _options.AppVersion,
//             ClientVersion = clientAssembly.Version,
//             HostName = Environment.MachineName,
//             Metadata = metadata,
//             Timestamp = DateTime.UtcNow
//         };

//         try
//         {
//             var jsonBody = Json.ToString(deepSightTelemetryDto);

//             using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, urlPath);

//             httpRequestMessage.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

//             if (!String.IsNullOrEmpty(traceSpan))
//             {
//                 httpRequestMessage.Headers.Add("tracespan", traceSpan);
//             }

//             _httpClient.SendAsync(httpRequestMessage).Wait(250);
//         }
//         catch
//         {
//         }
//     }
//     public override void TrackDependency(String name,
//                                          String type,
//                                          String target,
//                                          Boolean success,
//                                          Double duration,
//                                          Byte[] dataInput = null,
//                                          Byte[] dataOutput = null,
//                                          IDictionary<String, Object> metadata = null)
//     {
//         SendTelemetryData("dependencies", new Dictionary<String, Object>
//         {
//             { "dataInput", dataInput == null ? null : Convert.ToBase64String(dataInput) },
//             { "dataOutput", dataOutput == null ? null : Convert.ToBase64String(dataOutput) },
//             { "duration", duration },
//             { "name", name },
//             { "success", success },
//             { "target", target },
//             { "type", type }
//         });
//     }
//     public override void TrackEvent(String name,
//                                     IDictionary<String, Object> metadata = null)
//     {
//         SendTelemetryData("events", new Dictionary<String, Object>
//         {
//             { "name", name }
//         });
//     }
//     public override void TrackMetric(String name,
//                                      String dimension,
//                                      Double value,
//                                      IDictionary<String, Object> metadata = null)
//     {
//         SendTelemetryData("metrics", new Dictionary<String, Object>
//         {
//             { "dimension", dimension },
//             { "name", name },
//             { "value", value }
//         });
//     }
//     public override void TrackPageView(String name,
//                                        String deviceType,
//                                        Uri uri = null,
//                                        String userAgent = null,
//                                        IDictionary<String, Object> metadata = null)
//     {
//         SendTelemetryData("pageviews", new Dictionary<String, Object>
//         {
//             { "deviceType", deviceType },
//             { "name", name },
//             { "url", uri?.ToString() },
//             { "userAgent", userAgent }
//         });
//     }
//     public override void TrackRequest(String name,
//                                       String endpoint,
//                                       String type,
//                                       Boolean success,
//                                       Double duration,
//                                       IPAddress ipAddress,
//                                       Int32 statusCode,
//                                       Byte[] dataInput = null,
//                                       Byte[] dataOutput = null,
//                                       IDictionary<String, Object> metadata = null)
//     {
//         SendTelemetryData("requests", new Dictionary<String, Object>
//         {
//             { "dataInput", dataInput == null ? null : Convert.ToBase64String(dataInput) },
//             { "dataOutput", dataOutput == null ? null : Convert.ToBase64String(dataOutput) },
//             { "duration", duration },
//             { "endpoint", endpoint },
//             { "ipAddress", ipAddress?.ToString() },
//             { "name", name },
//             { "statusCode", statusCode },
//             { "success", success },
//             { "type", type }
//         });
//     }
//     public override void TrackTestResult(String name,
//                                          Boolean success,
//                                          Double duration,
//                                          String message = null,
//                                          IDictionary<String, Object> metadata = null)
//     {
//         SendTelemetryData("tests", new Dictionary<String, Object>
//         {
//             { "duration", duration },
//             { "message", message },
//             { "name", name },
//             { "success", success }
//         });
//     }
// }