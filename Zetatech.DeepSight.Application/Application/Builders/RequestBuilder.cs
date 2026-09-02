// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.Net;
// using Zetatech.Accelerate.Serialization;
// using Zetatech.DeepSight.Application.Dtos;
// using Zetatech.DeepSight.Domain.Entities;

// namespace Zetatech.DeepSight.Application.Builders;

// public static class RequestDtoBuilder
// {
//     public static RequestDto ToRequestDto(this RequestEntity requestEntity)
//     {
//         var requestDto = new RequestDto
//         {
//             AppName = requestEntity.AppName,
//             DataInput = requestEntity.DataInput,
//             DataOutput = requestEntity.DataOutput,
//             Duration = requestEntity.Duration,
//             EndPoint = requestEntity.EndPoint,
//             HostName = requestEntity.HostName,
//             Name = requestEntity.Name,
//             SpanId = requestEntity.SpanId,
//             StatusCode = requestEntity.StatusCode,
//             Success = requestEntity.Success,
//             TenantId = requestEntity.TenantId,
//             Timestamp = requestEntity.Timestamp,
//             TraceId = requestEntity.TraceId,
//             Type = requestEntity.Type
//         };

//         if (Version.TryParse(requestEntity.AppVersion, out var appVersion))
//         {
//             requestDto.AppVersion = appVersion;
//         }

//         if (IPAddress.TryParse(requestEntity.ClientIpAddress, out var clientIpAddress))
//         {
//             requestDto.ClientIpAddress = clientIpAddress;
//         }

//         if (Version.TryParse(requestEntity.ClientVersion, out var clientVersion))
//         {
//             requestDto.ClientVersion = clientVersion;
//         }

//         if (!String.IsNullOrEmpty(requestEntity.Metadata))
//         {
//             requestDto.Metadata = Json.ToObject<IDictionary<String, Object>>(requestEntity.Metadata);
//         }

//         return requestDto;
//     }
//     public static RequestEntity ToRequestEntity(this DeepSightDto deepSightDto)
//     {
//         var requestEntity = new RequestEntity
//         {
//             AppName = deepSightDto.AppName,
//             HostName = deepSightDto.HostName,
//             TenantId = deepSightDto.TenantId,
//             Timestamp = deepSightDto.Timestamp
//         };

//         if (deepSightDto.AppVersion != null)
//         {
//             requestEntity.AppVersion = $"{deepSightDto.AppVersion}";
//         }

//         if (deepSightDto.ClientIpAddress != null)
//         {
//             requestEntity.ClientIpAddress = $"{deepSightDto.ClientIpAddress}";
//         }

//         if (deepSightDto.ClientVersion != null)
//         {
//             requestEntity.ClientVersion = $"{deepSightDto.ClientVersion}";
//         }

//         if (deepSightDto.Metadata != null)
//         {
//             if (deepSightDto.Metadata.ContainsKey("dataInput"))
//             {
//                 if (deepSightDto.Metadata["dataInput"] != null)
//                 {
//                     var base64String = deepSightDto.Metadata["dataInput"].ToString();
//                     requestEntity.DataInput = Convert.FromBase64String(base64String);
//                 }

//                 deepSightDto.Metadata.Remove("dataInput");
//             }

//             if (deepSightDto.Metadata.ContainsKey("dataOutput"))
//             {
//                 if (deepSightDto.Metadata["dataOutput"] != null)
//                 {
//                     var base64String = deepSightDto.Metadata["dataOutput"].ToString();
//                     requestEntity.DataOutput = Convert.FromBase64String(base64String);
//                 }

//                 deepSightDto.Metadata.Remove("dataOutput");
//             }

//             if (deepSightDto.Metadata.ContainsKey("duration"))
//             {
//                 if (deepSightDto.Metadata["duration"] != null)
//                 {
//                     requestEntity.Duration = Double.Parse(deepSightDto.Metadata["duration"].ToString(), CultureInfo.InvariantCulture);
//                 }

//                 deepSightDto.Metadata.Remove("duration");
//             }

//             if (deepSightDto.Metadata.ContainsKey("endpoint"))
//             {
//                 requestEntity.EndPoint = deepSightDto.Metadata["endpoint"]?.ToString();
//                 deepSightDto.Metadata.Remove("endpoint");
//             }

//             if (deepSightDto.Metadata.ContainsKey("ipAddress"))
//             {
//                 requestEntity.IPAddress = deepSightDto.Metadata["ipAddress"]?.ToString();
//                 deepSightDto.Metadata.Remove("ipAddress");
//             }

//             if (deepSightDto.Metadata.ContainsKey("name"))
//             {
//                 requestEntity.Name = deepSightDto.Metadata["name"]?.ToString();
//                 deepSightDto.Metadata.Remove("name");
//             }

//             if (deepSightDto.Metadata.ContainsKey("statusCode"))
//             {
//                 if (deepSightDto.Metadata["statusCode"] != null)
//                 {
//                     requestEntity.StatusCode = Int32.Parse(deepSightDto.Metadata["statusCode"].ToString(), CultureInfo.InvariantCulture);
//                 }

//                 deepSightDto.Metadata.Remove("statusCode");
//             }

//             if (deepSightDto.Metadata.ContainsKey("success"))
//             {
//                 if (deepSightDto.Metadata["success"] != null)
//                 {
//                     requestEntity.Success = Boolean.Parse(deepSightDto.Metadata["success"].ToString());
//                 }

//                 deepSightDto.Metadata.Remove("success");
//             }

//             if (deepSightDto.Metadata.ContainsKey("type"))
//             {
//                 requestEntity.Type = deepSightDto.Metadata["type"]?.ToString();
//                 deepSightDto.Metadata.Remove("type");
//             }

//             if (deepSightDto.Metadata.Count > 0)
//             {
//                 requestEntity.Metadata = Json.ToString(deepSightDto.Metadata);
//             }
//         }

//         return requestEntity;
//     }
// }
