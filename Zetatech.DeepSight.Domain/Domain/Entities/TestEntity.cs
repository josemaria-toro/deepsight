using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zetatech.DeepSight.Domain.Abstractions;

namespace Zetatech.DeepSight.Domain.Entities;

[Table("tests", Schema = "deepsight")]
public sealed class TestEntity : BaseDeepSightEntity
{
    [Required]
    [Column("c_dbl_duration")]
    public Double Duration { get; set; }
    [Required]
    [MaxLength(4096)]
    [Column("c_str_message")]
    public String Message { get; set; }
    [Required]
    [MaxLength(128)]
    [Column("c_str_name")]
    public String Name { get; set; }
    [Required]
    [Column("c_bln_success")]
    public Boolean Success { get; set; }
}