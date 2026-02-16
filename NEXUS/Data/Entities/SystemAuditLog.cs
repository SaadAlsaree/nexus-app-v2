using System.ComponentModel.DataAnnotations;

namespace NEXUS.Data.Entities;

public class SystemAuditLog
{
    [Key]
    public long LogId { get; set; }
    public Guid UserId { get; set; } // من قام بالفعل
    [MaxLength(50)]
    public string ActionType { get; set; } = string.Empty; // (Insert, Read, Delete)
    [MaxLength(50)]
    public string TableName { get; set; } = string.Empty;
    [MaxLength(50)]
    public string? RecordId { get; set; }
    [MaxLength(50)]
    public string? OldValues { get; set; } // JSON
    [MaxLength(50)]
    public string? NewValues { get; set; } = string.Empty;// JSON
    [MaxLength(50)]
    public string IpAddress { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
