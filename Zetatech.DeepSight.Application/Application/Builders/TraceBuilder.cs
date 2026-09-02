using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;
using Zetatech.Accelerate.Serialization;
using Zetatech.DeepSight.Application.Dtos;
using Zetatech.DeepSight.Domain.Entities;

namespace Zetatech.DeepSight.Application.Builders;

public static class TraceDtoBuilder
{
    public static TraceDto ToTraceDto(this TraceEntity traceEntity)
    {
        var traceDto = new TraceDto
        {
            AppName = traceEntity.AppName,
            Category = traceEntity.Category,
            HostName = traceEntity.HostName,
            Message = traceEntity.Message,
            SpanId = traceEntity.SpanId,
            TenantId = traceEntity.TenantId,
            Timestamp = traceEntity.Timestamp,
            TraceId = traceEntity.TraceId
        };

        if (Version.TryParse(traceEntity.AppVersion, out var appVersion))
        {
            traceDto.AppVersion = appVersion;
        }

        if (IPAddress.TryParse(traceEntity.ClientIpAddress, out var clientIpAddress))
        {
            traceDto.ClientIpAddress = clientIpAddress;
        }

        if (Version.TryParse(traceEntity.ClientVersion, out var clientVersion))
        {
            traceDto.ClientVersion = clientVersion;
        }

        if (!String.IsNullOrEmpty(traceEntity.Metadata))
        {
            traceDto.Metadata = Json.ToObject<IDictionary<String, Object>>(traceEntity.Metadata);
        }

        if (Enum.TryParse<LogLevel>(traceEntity.Severity, true, out var severity))
        {
            traceDto.Severity = severity;
        }

        return traceDto;
    }
    public static TraceEntity ToTraceEntity(this DeepSightDto deepSightDto)
    {
        var traceEntity = new TraceEntity
        {
            AppName = deepSightDto.AppName,
            HostName = deepSightDto.HostName,
            TenantId = deepSightDto.TenantId,
            Timestamp = deepSightDto.Timestamp
        };

        if (deepSightDto.AppVersion != null)
        {
            traceEntity.AppVersion = $"{deepSightDto.AppVersion}";
        }

        if (deepSightDto.ClientIpAddress != null)
        {
            traceEntity.ClientIpAddress = $"{deepSightDto.ClientIpAddress}";
        }

        if (deepSightDto.ClientVersion != null)
        {
            traceEntity.ClientVersion = $"{deepSightDto.ClientVersion}";
        }

        if (deepSightDto.Metadata != null)
        {
            if (deepSightDto.Metadata.ContainsKey("category"))
            {
                traceEntity.Category = deepSightDto.Metadata["category"]?.ToString();
                deepSightDto.Metadata.Remove("category");
            }

            if (deepSightDto.Metadata.ContainsKey("message"))
            {
                traceEntity.Message = deepSightDto.Metadata["message"]?.ToString();
                deepSightDto.Metadata.Remove("message");
            }

            if (deepSightDto.Metadata.ContainsKey("severity"))
            {
                traceEntity.Severity = deepSightDto.Metadata["severity"]?.ToString();
                deepSightDto.Metadata.Remove("severity");
            }

            if (deepSightDto.Metadata.Count > 0)
            {
                traceEntity.Metadata = Json.ToString(deepSightDto.Metadata);
            }
        }

        return traceEntity;
    }
}
