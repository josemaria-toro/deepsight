using System;
using Zetatech.Accelerate.Application.Abstractions;

namespace Zetatech.DeepSight.Application.Dtos;

public sealed record TenantDto : BaseDto
{
    public Boolean Enabled { get; set; }
    public Guid? Id { get; set; }
    public String Name { get; set; }
}