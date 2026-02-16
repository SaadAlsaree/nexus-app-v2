namespace NEXUS.Events;

public record SuspectAddedToCaseEvent(
    Guid SuspectId,       // معرف المتهم
    string CaseNumber,    // رقم القضية (44/2026)
    string Role,          // دوره: "المنفذ"، "الممول"، "شاهد"
    string CaseStatus     // حالة القضية: "قيد التحقيق"، "مغلقة"
);