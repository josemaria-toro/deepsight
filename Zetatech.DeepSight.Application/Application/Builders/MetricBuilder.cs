// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.Net;
// using Zetatech.Accelerate.Serialization;
// using Zetatech.DeepSight.Application.Dtos;
// using Zetatech.DeepSight.Domain.Entities;

// namespace Zetatech.DeepSight.Application.Builders;

// public static class MetricBuilder
// {
//     public static MetricDto ToMetricDto(this MetricEntity metricEntity)
//     {
//         var metricDto = new MetricDto
//         {
//             AppName = metricEntity.AppName,
//             Dimension = metricEntity.Dimension,
//             HostName = metricEntity.HostName,
//             Name = metricEntity.Name,
//             SpanId = metricEntity.SpanId,
//             TenantId = metricEntity.TenantId,
//             Timestamp = metricEntity.Timestamp,
//             TraceId = metricEntity.TraceId,
//             Value = metricEntity.Value
//         };

//         if (Version.TryParse(metricEntity.AppVersion, out var appVersion))
//         {
//             metricDto.AppVersion = appVersion;
//         }

//         if (IPAddress.TryParse(metricEntity.ClientIpAddress, out var clientIpAddress))
//         {
//             metricDto.ClientIpAddress = clientIpAddress;
//         }

//         if (Version.TryParse(metricEntity.ClientVersion, out var clientVersion))
//         {
//             metricDto.ClientVersion = clientVersion;
//         }

//         if (!String.IsNullOrEmpty(metricEntity.Metadata))
//         {
//             metricDto.Metadata = Json.ToObject<IDictionary<String, Object>>(metricEntity.Metadata);
//         }

//         return metricDto;
//     }
//     public static MetricEntity ToMetricEntity(this DeepSightDto deepSightDto)
//     {
//         var metricEntity = new MetricEntity
//         {
//             AppName = deepSightDto.AppName,
//             HostName = deepSightDto.HostName,
//             TenantId = deepSightDto.TenantId,
//             Timestamp = deepSightDto.Timestamp
//         };

//         if (deepSightDto.AppVersion != null)
//         {
//             metricEntity.AppVersion = $"{deepSightDto.AppVersion}";
//         }

//         if (deepSightDto.ClientIpAddress != null)
//         {
//             metricEntity.ClientIpAddress = $"{deepSightDto.ClientIpAddress}";
//         }

//         if (deepSightDto.ClientVersion != null)
//         {
//             metricEntity.ClientVersion = $"{deepSightDto.ClientVersion}";
//         }

//         if (deepSightDto.Metadata != null)
//         {
//             if (deepSightDto.Metadata.ContainsKey("dimension"))
//             {
//                 metricEntity.Dimension = deepSightDto.Metadata["dimension"]?.ToString();
//                 deepSightDto.Metadata.Remove("dimension");
//             }

//             if (deepSightDto.Metadata.ContainsKey("name"))
//             {
//                 metricEntity.Name = deepSightDto.Metadata["name"]?.ToString();
//                 deepSightDto.Metadata.Remove("name");
//             }

//             if (deepSightDto.Metadata.ContainsKey("value"))
//             {
//                 if (deepSightDto.Metadata["value"] != null)
//                 {
//                     metricEntity.Value = Double.Parse(deepSightDto.Metadata["value"].ToString(), CultureInfo.InvariantCulture);
//                 }

//                 deepSightDto.Metadata.Remove("value");
//             }

//             if (deepSightDto.Metadata.Count > 0)
//             {
//                 metricEntity.Metadata = Json.ToString(deepSightDto.Metadata);
//             }
//         }

//         return metricEntity;
//     }
// }
