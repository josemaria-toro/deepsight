using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zetatech.DeepSight.Domain.Abstractions;

namespace Zetatech.DeepSight.Domain.Entities;

[Table("events", Schema = "deepsight")]
public sealed class EventEntity : BaseDeepSightEntity
{
    [Required]
    [MaxLength(128)]
    [Column("c_str_name")]
    public String Name { get; set; }
}