using System;
using Zetatech.DeepSight.Application.Dtos;
using Zetatech.DeepSight.Domain.Entities;

namespace Zetatech.DeepSight.Application.Builders;

public static class DependencyBuilder
{
    public static DependencyDto Build(this DependencyEntity dependencyEntity)
    {
        var dependencyDto = new DependencyDto
        {
            AppName = dependencyEntity.AppName,
            AppVersion = dependencyEntity.AppVersion,
            ClientIpAddress = dependencyEntity.ClientIpAddress,
            ClientVersion = dependencyEntity.ClientVersion,
            DataInput = dependencyEntity.DataInput == null ? null : Convert.ToBase64String(dependencyEntity.DataInput),
            DataOutput = dependencyEntity.DataOutput == null ? null : Convert.ToBase64String(dependencyEntity.DataOutput),
            Duration = dependencyEntity.Duration,
            HostName = dependencyEntity.HostName,
            Metadata = dependencyEntity.Metadata,
            Name = dependencyEntity.Name,
            SpanId = dependencyEntity.SpanId,
            Success = dependencyEntity.Success,
            Target = dependencyEntity.Target,
            TenantId = dependencyEntity.TenantId,
            Timestamp = dependencyEntity.Timestamp,
            TraceId = dependencyEntity.TraceId,
            Type = dependencyEntity.Type
        };

        return dependencyDto;
    }
    public static DependencyEntity Build(this DependencyDto dependencyDto)
    {
        return new DependencyEntity
        {
            AppName = dependencyDto.AppName,
            AppVersion = dependencyDto.AppVersion,
            ClientIpAddress = dependencyDto.ClientIpAddress,
            ClientVersion = dependencyDto.ClientVersion,
            DataInput = String.IsNullOrEmpty(dependencyDto.DataInput) ? null : Convert.FromBase64String(dependencyDto.DataInput),
            DataOutput = String.IsNullOrEmpty(dependencyDto.DataOutput) ? null : Convert.FromBase64String(dependencyDto.DataOutput),
            Duration = dependencyDto.Duration.GetValueOrDefault(),
            HostName = dependencyDto.HostName,
            Metadata = dependencyDto.Metadata,
            Name = dependencyDto.Name,
            SpanId = dependencyDto.SpanId,
            Success = dependencyDto.Success.GetValueOrDefault(),
            Target = dependencyDto.Target,
            TenantId = dependencyDto.TenantId.GetValueOrDefault(),
            Timestamp = dependencyDto.Timestamp.GetValueOrDefault(),
            Type = dependencyDto.Type,
            TraceId = dependencyDto.TraceId
        };
    }
}
