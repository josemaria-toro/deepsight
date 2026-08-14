using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Zetatech.Accelerate.Data.Abstractions;

namespace Zetatech.DeepSight.Domain.Abstractions;

public abstract class BaseDeepSightEntity : BaseEntity
{
    [Required]
    [MaxLength(128)]
    [Column("c_str_app_name")]
    public String AppName { get; set; }
    [Required]
    [MaxLength(32)]
    [Column("c_str_app_version")]
    public String AppVersion { get; set; }
    [Required]
    [MaxLength(15)]
    [Column("c_str_client_ip_address")]
    public String ClientIpAddress { get; set; }
    [Required]
    [MaxLength(32)]
    [Column("c_str_client_version")]
    public String ClientVersion { get; set; }
    [MaxLength(128)]
    [Column("c_str_host_name")]
    public String HostName { get; set; }
    [Column("c_jsn_metadata", TypeName = "jsonb")]
    public String Metadata { get; set; }
    [Required]
    [Column("c_uid_tenant_id")]
    public Guid TenantId { get; set; }
    [Required]
    [Column("c_tsp_timestamp")]
    public DateTime Timestamp { get; set; }
}