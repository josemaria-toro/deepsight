using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Zetatech.Accelerate.Serialization;
using Zetatech.DeepSight.Application.Dtos;
using Zetatech.DeepSight.Domain.Entities;

namespace Zetatech.DeepSight.Application.Builders;

public static class DependencyBuilder
{
    public static DependencyDto ToDependencyDto(this DependencyEntity dependencyEntity)
    {
        var dependencyDto = new DependencyDto
        {
            AppName = dependencyEntity.AppName,
            DataInput = dependencyEntity.DataInput,
            DataOutput = dependencyEntity.DataOutput,
            Duration = dependencyEntity.Duration,
            HostName = dependencyEntity.HostName,
            Name = dependencyEntity.Name,
            Success = dependencyEntity.Success,
            Target = dependencyEntity.Target,
            TenantId = dependencyEntity.TenantId,
            Timestamp = dependencyEntity.Timestamp,
            Type = dependencyEntity.Type
        };

        if (Version.TryParse(dependencyEntity.AppVersion, out var appVersion))
        {
            dependencyDto.AppVersion = appVersion;
        }

        if (IPAddress.TryParse(dependencyEntity.ClientIpAddress, out var clientIpAddress))
        {
            dependencyDto.ClientIpAddress = clientIpAddress;
        }

        if (Version.TryParse(dependencyEntity.ClientVersion, out var clientVersion))
        {
            dependencyDto.ClientVersion = clientVersion;
        }

        if (!String.IsNullOrEmpty(dependencyEntity.Metadata))
        {
            dependencyDto.Metadata = Json.ToObject<IDictionary<String, Object>>(dependencyEntity.Metadata);
        }

        return dependencyDto;
    }
    public static DependencyEntity ToDependencyEntity(this DeepSightDto deepSightDto)
    {
        var dependencyEntity = new DependencyEntity
        {
            AppName = deepSightDto.AppName,
            HostName = deepSightDto.HostName,
            TenantId = deepSightDto.TenantId,
            Timestamp = deepSightDto.Timestamp
        };

        if (deepSightDto.AppVersion != null)
        {
            dependencyEntity.AppVersion = $"{deepSightDto.AppVersion}";
        }

        if (deepSightDto.ClientIpAddress != null)
        {
            dependencyEntity.ClientIpAddress = $"{deepSightDto.ClientIpAddress}";
        }

        if (deepSightDto.ClientVersion != null)
        {
            dependencyEntity.ClientVersion = $"{deepSightDto.ClientVersion}";
        }

        if (deepSightDto.Metadata != null)
        {
            if (deepSightDto.Metadata.ContainsKey("dataInput"))
            {
                if (deepSightDto.Metadata["dataInput"] != null)
                {
                    var base64String = deepSightDto.Metadata["dataInput"].ToString();
                    dependencyEntity.DataInput = Convert.FromBase64String(base64String);
                }

                deepSightDto.Metadata.Remove("dataInput");
            }

            if (deepSightDto.Metadata.ContainsKey("dataOutput"))
            {
                if (deepSightDto.Metadata["dataOutput"] != null)
                {
                    var base64String = deepSightDto.Metadata["dataOutput"].ToString();
                    dependencyEntity.DataOutput = Convert.FromBase64String(base64String);
                }

                deepSightDto.Metadata.Remove("dataOutput");
            }

            if (deepSightDto.Metadata.ContainsKey("duration"))
            {
                if (deepSightDto.Metadata["duration"] != null)
                {
                    dependencyEntity.Duration = Double.Parse(deepSightDto.Metadata["duration"].ToString(), CultureInfo.InvariantCulture);
                }

                deepSightDto.Metadata.Remove("duration");
            }

            if (deepSightDto.Metadata.ContainsKey("name"))
            {
                dependencyEntity.Name = deepSightDto.Metadata["name"]?.ToString();
                deepSightDto.Metadata.Remove("name");
            }

            if (deepSightDto.Metadata.ContainsKey("success"))
            {
                if (deepSightDto.Metadata["success"] != null)
                {
                    dependencyEntity.Success = Boolean.Parse(deepSightDto.Metadata["success"].ToString());
                }

                deepSightDto.Metadata.Remove("success");
            }

            if (deepSightDto.Metadata.ContainsKey("target"))
            {
                dependencyEntity.Target = deepSightDto.Metadata["target"]?.ToString();
                deepSightDto.Metadata.Remove("target");
            }

            if (deepSightDto.Metadata.ContainsKey("type"))
            {
                dependencyEntity.Type = deepSightDto.Metadata["type"]?.ToString();
                deepSightDto.Metadata.Remove("type");
            }

            if (deepSightDto.Metadata.Count > 0)
            {
                dependencyEntity.Metadata = Json.ToString(deepSightDto.Metadata);
            }
        }

        return dependencyEntity;
    }
}
