using NEXUS.Data.Enums;

namespace NEXUS.Dtos;

public class AnalysisReport
{
    // هل وجدنا شيئاً مثيراً للريبة؟ (True = خطر/تنبيه)
    public bool IsFlagged { get; set; }

    // مستوى الخطورة (من الـ Enum الذي أنشأناه سابقاً)
    public AlertLevel AlertLevel { get; set; } = AlertLevel.Low;

    // رسالة التقرير للمحقق
    // مثال: "هذا الرقم مطابق لرقم والي بغداد في قائمة المراقبة"
    public string Message { get; set; } = string.Empty;

    // مصدر الخطر (هل هو قائمة المراقبة؟ أم تحليل Neo4j؟)
    public string Source { get; set; } = string.Empty;// "Watchlist" or "GraphPattern"

    // توصية النظام (ماذا يجب أن يفعل المحقق؟)
    public string Recommendation { get; set; } = string.Empty; // "يجب إبلاغ الأمن الوطني فوراً"
}