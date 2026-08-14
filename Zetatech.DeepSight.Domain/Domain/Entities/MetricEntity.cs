using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zetatech.DeepSight.Domain.Abstractions;

namespace Zetatech.DeepSight.Domain.Entities;

[Table("metrics", Schema = "deepsight")]
public sealed class MetricEntity : BaseDeepSightEntity
{
    [Required]
    [MaxLength(128)]
    [Column("c_str_dimension")]
    public String Dimension { get; set; }
    [Required]
    [MaxLength(128)]
    [Column("c_str_name")]
    public String Name { get; set; }
    [Required]
    [Column("c_dbl_value")]
    public Double Value { get; set; }
}