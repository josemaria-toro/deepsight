using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zetatech.DeepSight.Domain.Abstractions;

namespace Zetatech.DeepSight.Domain.Entities;

[Table("requests", Schema = "deepsight")]
public sealed class RequestEntity : BaseDeepSightEntity
{
    [Column("c_bta_data_input")]
    public Byte[] DataInput { get; set; }
    [Column("c_bta_data_output")]
    public Byte[] DataOutput { get; set; }
    [Required]
    [Column("c_dbl_duration")]
    public Double Duration { get; set; }
    [Required]
    [MaxLength(1024)]
    [Column("c_str_endpoint")]
    public String EndPoint { get; set; }
    [Required]
    [MaxLength(15)]
    [Column("c_str_ip_address")]
    public String IPAddress { get; set; }
    [Required]
    [MaxLength(128)]
    [Column("c_str_name")]
    public String Name { get; set; }
    [Column("c_int_status_code")]
    public Int32? StatusCode { get; set; }
    [Required]
    [Column("c_bln_success")]
    public Boolean Success { get; set; }
    [Required]
    [MaxLength(16)]
    [Column("c_str_type")]
    public String Type { get; set; }
}