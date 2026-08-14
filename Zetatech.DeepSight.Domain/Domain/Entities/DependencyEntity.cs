using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zetatech.DeepSight.Domain.Abstractions;

namespace Zetatech.DeepSight.Domain.Entities;

[Table("dependencies", Schema = "deepsight")]
public sealed class DependencyEntity : BaseDeepSightEntity
{
    [Column("c_bta_data_input")]
    public Byte[] DataInput { get; set; }
    [Column("c_bta_data_output")]
    public Byte[] DataOutput { get; set; }
    [Required]
    [Column("c_dbl_duration")]
    public Double Duration { get; set; }
    [Required]
    [MaxLength(128)]
    [Column("c_str_name")]
    public String Name { get; set; }
    [Required]
    [Column("c_bln_success")]
    public Boolean Success { get; set; }
    [Required]
    [MaxLength(128)]
    [Column("c_str_target")]
    public String Target { get; set; }
    [Required]
    [MaxLength(32)]
    [Column("c_str_type")]
    public String Type { get; set; }
}