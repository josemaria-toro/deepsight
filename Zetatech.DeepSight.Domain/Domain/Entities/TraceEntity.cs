using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zetatech.DeepSight.Domain.Abstractions;

namespace Zetatech.DeepSight.Domain.Entities;

[Table("traces", Schema = "deepsight")]
public sealed class TraceEntity : BaseDeepSightEntity
{
    [Required]
    [MaxLength(128)]
    [Column("c_str_category")]
    public String Category { get; set; }
    [Required]
    [MaxLength(4096)]
    [Column("c_str_message")]
    public String Message { get; set; }
    [Required]
    [MaxLength(16)]
    [Column("c_str_severity")]
    public String Severity { get; set; }
}