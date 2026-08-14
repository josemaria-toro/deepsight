using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zetatech.DeepSight.Domain.Abstractions;

namespace Zetatech.DeepSight.Domain.Entities;

[Table("pageviews", Schema = "deepsight")]
public sealed class PageViewEntity : BaseDeepSightEntity
{
    [Required]
    [MaxLength(32)]
    [Column("c_str_device_type")]
    public String DeviceType { get; set; }
    [Required]
    [MaxLength(128)]
    [Column("c_str_name")]
    public String Name { get; set; }
    [MaxLength(1024)]
    [Column("c_str_url")]
    public String Url { get; set; }
    [MaxLength(1024)]
    [Column("c_str_user_agent")]
    public String UserAgent { get; set; }
}