using NEXUS.Data.Common;
using NEXUS.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace NEXUS.Data.Entities;

public class Case : BaseEntity
{
    [Required]
    [MaxLength(20)]
    public string CaseFileNumber { get; set; } = string.Empty; // رقم القضية 2024/101
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    public DateOnly OpenDate { get; set; }
    public CaseStatus Status { get; set; } // (قيد التحقيق، مغلقة)
    [MaxLength(50)]
    public string? InvestigatingOfficer { get; set; } = string.Empty; // (اسم المسئول)
    public PriorityLevel Priority { get; set; } // (عالية، منخفضة)
    public int SuspectsCount { get; set; }

    public ICollection<CaseSuspect> SuspectsInvolved { get; set; } = default!;
    public ICollection<EvidenceAttachment> Attachments { get; set; } = default!;
}
