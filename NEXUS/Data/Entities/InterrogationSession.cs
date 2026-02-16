using NEXUS.Data.Common;
using NEXUS.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS.Data.Entities;

[Table("InterrogationSessions")]
public class InterrogationSession : BaseEntity
{
    // الربط: هذه الجلسة تابعة لأي متهم وفي أي قضية؟
    public Guid SuspectId { get; set; }
    public Suspect Suspect { get; set; } = default!;

    public Guid CaseId { get; set; }
    public Case Case { get; set; } = default!;

    // تفاصيل الجلسة
    public DateOnly SessionDate { get; set; } // متى تمت؟

    [MaxLength(100)]
    public string InterrogatorName { get; set; } = string.Empty; // الضابط المحقق

    public string Location { get; set; } = string.Empty; // مكان التحقيق (مديرية كذا، سجن كذا)

    // نوع الجلسة (إفادة أولية، تدوين قضائي، كشف دلالة)
    public InterrogationType SessionType { get; set; }

    // النص الكامل للإفادة (هنا يتم البحث النصي والتحليل)
    [Required]
    public string Content { get; set; } = string.Empty;
    public string SummaryContent { get; set; } = string.Empty;

    // نتيجة الجلسة (اعترف، أنكر، صمت)
    public InterrogationOutcome Outcome { get; set; }

    // ملاحظات المحقق الخاصة (لا تظهر في الإفادة الرسمية)
    public string? InvestigatorNotes { get; set; }

    // هل تم تصديق الأقوال قضائياً؟
    public bool IsRatifiedByJudge { get; set; } = false;
}