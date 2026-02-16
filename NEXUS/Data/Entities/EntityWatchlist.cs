using NEXUS.Data.Common;
using NEXUS.Data.Enums;
using System.ComponentModel.DataAnnotations;
namespace NEXUS.Data.Entities;

public class EntityWatchlist : BaseEntity
{
    [MaxLength(50)]
    public string Keyword { get; set; } = string.Empty; // (رقم هاتف، اسم)
    public AlertLevel AlertLevel { get; set; } = AlertLevel.Low; // (High, Medium)
    [MaxLength(500)]
    public string? Reason { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
