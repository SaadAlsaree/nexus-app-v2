using NEXUS.Data.Enums;

namespace NEXUS.Events;

public record InterrogationSessionCreatedEvent(
        Guid SessionId,            // معرف الجلسة
        Guid SuspectId,            // معرف المتهم
        Guid CaseId,               // معرف القضية
        string Content,            // نص الاعتراف الكامل (هذا ما سيحلله الـ AI/Regex)
        string? SummaryContent,    // الملخص (إن وجد)
        InterrogationType Type,    // نوع الجلسة (إفادة، كشف دلالة...)
        InterrogationOutcome Outcome, // النتيجة (اعترف، أنكر)
        DateTime OccurredAt        // وقت حدوث التحقيق
    );