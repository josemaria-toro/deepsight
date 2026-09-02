// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.Net;
// using Zetatech.Accelerate.Serialization;
// using Zetatech.DeepSight.Application.Dtos;
// using Zetatech.DeepSight.Domain.Entities;

// namespace Zetatech.DeepSight.Application.Builders;

// public static class TestDtoBuilder
// {
//     public static TestDto ToTestDto(this TestEntity testEntity)
//     {
//         var testDto = new TestDto
//         {
//             AppName = testEntity.AppName,
//             Duration = testEntity.Duration,
//             HostName = testEntity.HostName,
//             Message = testEntity.Message,
//             Name = testEntity.Name,
//             SpanId = testEntity.SpanId,
//             Success = testEntity.Success,
//             TenantId = testEntity.TenantId,
//             Timestamp = testEntity.Timestamp,
//             TraceId = testEntity.TraceId
//         };

//         if (Version.TryParse(testEntity.AppVersion, out var appVersion))
//         {
//             testDto.AppVersion = appVersion;
//         }

//         if (IPAddress.TryParse(testEntity.ClientIpAddress, out var clientIpAddress))
//         {
//             testDto.ClientIpAddress = clientIpAddress;
//         }

//         if (Version.TryParse(testEntity.ClientVersion, out var clientVersion))
//         {
//             testDto.ClientVersion = clientVersion;
//         }

//         if (!String.IsNullOrEmpty(testEntity.Metadata))
//         {
//             testDto.Metadata = Json.ToObject<IDictionary<String, Object>>(testEntity.Metadata);
//         }

//         return testDto;
//     }
//     public static TestEntity ToTestEntity(this DeepSightDto deepSightDto)
//     {
//         var testEntity = new TestEntity
//         {
//             AppName = deepSightDto.AppName,
//             HostName = deepSightDto.HostName,
//             TenantId = deepSightDto.TenantId,
//             Timestamp = deepSightDto.Timestamp
//         };

//         if (deepSightDto.AppVersion != null)
//         {
//             testEntity.AppVersion = $"{deepSightDto.AppVersion}";
//         }

//         if (deepSightDto.ClientIpAddress != null)
//         {
//             testEntity.ClientIpAddress = $"{deepSightDto.ClientIpAddress}";
//         }

//         if (deepSightDto.ClientVersion != null)
//         {
//             testEntity.ClientVersion = $"{deepSightDto.ClientVersion}";
//         }

//         if (deepSightDto.Metadata != null)
//         {
//             if (deepSightDto.Metadata.ContainsKey("duration"))
//             {
//                 if (deepSightDto.Metadata["duration"] != null)
//                 {
//                     testEntity.Duration = Double.Parse(deepSightDto.Metadata["duration"].ToString(), CultureInfo.InvariantCulture);
//                 }

//                 deepSightDto.Metadata.Remove("duration");
//             }

//             if (deepSightDto.Metadata.ContainsKey("message"))
//             {
//                 testEntity.Message = deepSightDto.Metadata["message"]?.ToString();
//                 deepSightDto.Metadata.Remove("message");
//             }

//             if (deepSightDto.Metadata.ContainsKey("name"))
//             {
//                 testEntity.Name = deepSightDto.Metadata["name"]?.ToString();
//                 deepSightDto.Metadata.Remove("name");
//             }

//             if (deepSightDto.Metadata.ContainsKey("success"))
//             {
//                 if (deepSightDto.Metadata["success"] != null)
//                 {
//                     testEntity.Success = Boolean.Parse(deepSightDto.Metadata["success"].ToString());
//                 }

//                 deepSightDto.Metadata.Remove("success");
//             }

//             if (deepSightDto.Metadata.Count > 0)
//             {
//                 testEntity.Metadata = Json.ToString(deepSightDto.Metadata);
//             }
//         }

//         return testEntity;
//     }
// }
