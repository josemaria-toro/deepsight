using System;
using System.Collections.Generic;
using System.Net;
using Zetatech.Accelerate.Serialization;
using Zetatech.DeepSight.Application.Dtos;
using Zetatech.DeepSight.Domain.Entities;

namespace Zetatech.DeepSight.Application.Builders;

public static class PageViewBuilder
{
    public static PageViewDto ToPageViewDto(this PageViewEntity pageViewEntity)
    {
        var pageViewDto = new PageViewDto
        {
            AppName = pageViewEntity.AppName,
            DeviceType = pageViewEntity.DeviceType,
            HostName = pageViewEntity.HostName,
            Name = pageViewEntity.Name,
            TenantId = pageViewEntity.TenantId,
            Timestamp = pageViewEntity.Timestamp,
            UserAgent = pageViewEntity.UserAgent
        };

        if (Version.TryParse(pageViewEntity.AppVersion, out var appVersion))
        {
            pageViewDto.AppVersion = appVersion;
        }

        if (IPAddress.TryParse(pageViewEntity.ClientIpAddress, out var clientIpAddress))
        {
            pageViewDto.ClientIpAddress = clientIpAddress;
        }

        if (Version.TryParse(pageViewEntity.ClientVersion, out var clientVersion))
        {
            pageViewDto.ClientVersion = clientVersion;
        }

        if (!String.IsNullOrEmpty(pageViewEntity.Metadata))
        {
            pageViewDto.Metadata = Json.ToObject<IDictionary<String, Object>>(pageViewEntity.Metadata);
        }

        if (Uri.TryCreate(pageViewEntity.Url, UriKind.RelativeOrAbsolute,  out var url))
        {
            pageViewDto.Url = url;
        }

        return pageViewDto;
    }
    public static PageViewEntity ToPageViewEntity(this DeepSightDto deepSightDto)
    {
        var pageViewEntity = new PageViewEntity
        {
            AppName = deepSightDto.AppName,
            HostName = deepSightDto.HostName,
            TenantId = deepSightDto.TenantId,
            Timestamp = deepSightDto.Timestamp
        };

        if (deepSightDto.AppVersion != null)
        {
            pageViewEntity.AppVersion = $"{deepSightDto.AppVersion}";
        }

        if (deepSightDto.ClientIpAddress != null)
        {
            pageViewEntity.ClientIpAddress = $"{deepSightDto.ClientIpAddress}";
        }

        if (deepSightDto.ClientVersion != null)
        {
            pageViewEntity.ClientVersion = $"{deepSightDto.ClientVersion}";
        }

        if (deepSightDto.Metadata != null)
        {
            if (deepSightDto.Metadata.ContainsKey("deviceType"))
            {
                pageViewEntity.DeviceType = deepSightDto.Metadata["deviceType"]?.ToString();
                deepSightDto.Metadata.Remove("deviceType");
            }

            if (deepSightDto.Metadata.ContainsKey("name"))
            {
                pageViewEntity.Name = deepSightDto.Metadata["name"]?.ToString();
                deepSightDto.Metadata.Remove("name");
            }

            if (deepSightDto.Metadata.ContainsKey("url"))
            {
                pageViewEntity.Url = deepSightDto.Metadata["url"]?.ToString();
                deepSightDto.Metadata.Remove("url");
            }

            if (deepSightDto.Metadata.ContainsKey("userAgent"))
            {
                pageViewEntity.UserAgent = deepSightDto.Metadata["userAgent"]?.ToString();
                deepSightDto.Metadata.Remove("userAgent");
            }

            if (deepSightDto.Metadata.Count > 0)
            {
                pageViewEntity.Metadata = Json.ToString(deepSightDto.Metadata);
            }
        }

        return pageViewEntity;
    }
}
