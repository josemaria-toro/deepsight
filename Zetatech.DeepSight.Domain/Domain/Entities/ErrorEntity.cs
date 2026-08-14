using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zetatech.DeepSight.Domain.Abstractions;

namespace Zetatech.DeepSight.Domain.Entities;

[Table("errors", Schema = "deepsight")]
public sealed class ErrorEntity : BaseDeepSightEntity
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
    [Required]
    [MaxLength(4096)]
    [Column("c_str_stack_trace")]
    public String StackTrace { get; set; }
    [Required]
    [MaxLength(128)]
    [Column("c_str_error_type")]
    public String Type { get; set; }
}