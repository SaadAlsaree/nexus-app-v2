using NEXUS.Data.Common;
using NEXUS.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace NEXUS.Data.Entities;

public class SystemAlert : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty; // عنوان التنبيه (مثلاً: "تطابق أمني")

    [Required]
    public string Message { get; set; } = string.Empty; // نص التنبيه التفصيلي

    public AlertLevel Level { get; set; } // مستوى الخطورة (High, Medium...)

    public bool IsRead { get; set; } = false; // هل تم الاطلاع عليه؟

    [MaxLength(200)]
    public string? Source { get; set; } // مصدر التنبيه (Graph, Watchlist)

    // (اختياري) لربط التنبيه بالمتهم أو الملف الذي سبب التنبيه، ليسهل الانتقال إليه
    public Guid? RelatedEntityId { get; set; }
}
