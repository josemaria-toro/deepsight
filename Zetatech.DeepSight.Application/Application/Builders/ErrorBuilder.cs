using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;
using Zetatech.Accelerate.Serialization;
using Zetatech.DeepSight.Application.Dtos;
using Zetatech.DeepSight.Domain.Entities;

namespace Zetatech.DeepSight.Application.Builders;

public static class ErrorBuilder
{
    public static ErrorDto ToErrorDto(this ErrorEntity errorEntity)
    {
        var errorDto = new ErrorDto
        {
            AppName = errorEntity.AppName,
            Category = errorEntity.Category,
            HostName = errorEntity.HostName,
            Message = errorEntity.Message,
            StackTrace = errorEntity.StackTrace,
            TenantId = errorEntity.TenantId,
            Timestamp = errorEntity.Timestamp,
            Type = errorEntity.Type
        };

        if (Version.TryParse(errorEntity.AppVersion, out var appVersion))
        {
            errorDto.AppVersion = appVersion;
        }

        if (IPAddress.TryParse(errorEntity.ClientIpAddress, out var clientIpAddress))
        {
            errorDto.ClientIpAddress = clientIpAddress;
        }

        if (Version.TryParse(errorEntity.ClientVersion, out var clientVersion))
        {
            errorDto.ClientVersion = clientVersion;
        }

        if (!String.IsNullOrEmpty(errorEntity.Metadata))
        {
            errorDto.Metadata = Json.ToObject<IDictionary<String, Object>>(errorEntity.Metadata);
        }

        if (Enum.TryParse<LogLevel>(errorEntity.Severity, true, out var severity))
        {
            errorDto.Severity = severity;
        }

        return errorDto;
    }
    public static ErrorEntity ToErrorDto(this DeepSightDto deepSightDto)
    {
        var errorEntity = new ErrorEntity
        {
            AppName = deepSightDto.AppName,
            HostName = deepSightDto.HostName,
            TenantId = deepSightDto.TenantId,
            Timestamp = deepSightDto.Timestamp
        };

        if (deepSightDto.AppVersion != null)
        {
            errorEntity.AppVersion = $"{deepSightDto.AppVersion}";
        }

        if (deepSightDto.ClientIpAddress != null)
        {
            errorEntity.ClientIpAddress = $"{deepSightDto.ClientIpAddress}";
        }

        if (deepSightDto.ClientVersion != null)
        {
            errorEntity.ClientVersion = $"{deepSightDto.ClientVersion}";
        }

        if (deepSightDto.Metadata != null)
        {
            if (deepSightDto.Metadata.ContainsKey("category"))
            {
                errorEntity.Category = deepSightDto.Metadata["category"]?.ToString();
                deepSightDto.Metadata.Remove("category");
            }

            if (deepSightDto.Metadata.ContainsKey("message"))
            {
                errorEntity.Message = deepSightDto.Metadata["message"]?.ToString();
                deepSightDto.Metadata.Remove("message");
            }

            if (deepSightDto.Metadata.ContainsKey("severity"))
            {
                errorEntity.Severity = deepSightDto.Metadata["severity"]?.ToString();
                deepSightDto.Metadata.Remove("severity");
            }

            if (deepSightDto.Metadata.ContainsKey("stackTrace"))
            {
                errorEntity.StackTrace = deepSightDto.Metadata["stackTrace"]?.ToString();
                deepSightDto.Metadata.Remove("stackTrace");
            }

            if (deepSightDto.Metadata.ContainsKey("typeName"))
            {
                errorEntity.Type = deepSightDto.Metadata["typeName"]?.ToString();
                deepSightDto.Metadata.Remove("typeName");
            }

            if (deepSightDto.Metadata.Count > 0)
            {
                errorEntity.Metadata = Json.ToString(deepSightDto.Metadata);
            }
        }

        return errorEntity;
    }
}
