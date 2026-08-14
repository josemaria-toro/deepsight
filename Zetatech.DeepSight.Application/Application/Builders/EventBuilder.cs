using System;
using System.Collections.Generic;
using System.Net;
using Zetatech.Accelerate.Serialization;
using Zetatech.DeepSight.Application.Dtos;
using Zetatech.DeepSight.Domain.Entities;

namespace Zetatech.DeepSight.Application.Builders;

public static class EventBuilder
{
    public static EventDto ToEventDto(this EventEntity eventEntity)
    {
        var eventDto = new EventDto
        {
            AppName = eventEntity.AppName,
            HostName = eventEntity.HostName,
            Name = eventEntity.Name,
            TenantId = eventEntity.TenantId,
            Timestamp = eventEntity.Timestamp
        };

        if (Version.TryParse(eventEntity.AppVersion, out var appVersion))
        {
            eventDto.AppVersion = appVersion;
        }

        if (IPAddress.TryParse(eventEntity.ClientIpAddress, out var clientIpAddress))
        {
            eventDto.ClientIpAddress = clientIpAddress;
        }

        if (Version.TryParse(eventEntity.ClientVersion, out var clientVersion))
        {
            eventDto.ClientVersion = clientVersion;
        }

        if (!String.IsNullOrEmpty(eventEntity.Metadata))
        {
            eventDto.Metadata = Json.ToObject<IDictionary<String, Object>>(eventEntity.Metadata);
        }

        return eventDto;
    }
    public static EventEntity ToEventEntity(this DeepSightDto deepSightDto)
    {
        var eventEntity = new EventEntity
        {
            AppName = deepSightDto.AppName,
            HostName = deepSightDto.HostName,
            TenantId = deepSightDto.TenantId,
            Timestamp = deepSightDto.Timestamp
        };

        if (deepSightDto.AppVersion != null)
        {
            eventEntity.AppVersion = $"{deepSightDto.AppVersion}";
        }

        if (deepSightDto.ClientIpAddress != null)
        {
            eventEntity.ClientIpAddress = $"{deepSightDto.ClientIpAddress}";
        }

        if (deepSightDto.ClientVersion != null)
        {
            eventEntity.ClientVersion = $"{deepSightDto.ClientVersion}";
        }

        if (deepSightDto.Metadata != null)
        {
            if (deepSightDto.Metadata.ContainsKey("name"))
            {
                eventEntity.Name = deepSightDto.Metadata["name"]?.ToString();
                deepSightDto.Metadata.Remove("name");
            }

            if (deepSightDto.Metadata.Count > 0)
            {
                eventEntity.Metadata = Json.ToString(deepSightDto.Metadata);
            }
        }

        return eventEntity;
    }
}
