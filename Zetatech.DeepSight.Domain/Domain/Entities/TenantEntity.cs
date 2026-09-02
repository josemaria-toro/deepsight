using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zetatech.Accelerate.Data.Abstractions;

namespace Zetatech.DeepSight.Domain.Entities;

[Table("tenants", Schema = "deepsight")]
public sealed class TenantEntity : BaseEntity
{
    [Required]
    [Column("c_bln_enabled")]
    public Boolean Enabled { get; set; }
    [Required]
    [MaxLength(128)]
    [Column("c_str_name")]
    public String Name { get; set; }
}