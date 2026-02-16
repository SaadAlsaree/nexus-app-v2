using NEXUS.Data.Enums;

namespace NEXUS.BackgroundJobs;

// الحدث الذي يطلق هذا الهاندلر (تمت إضافة جهة اتصال)
public record ContactAddedEvent(
    Guid ContactId,
    Guid SuspectId,
    string PhoneNumber,
    string ContactType
);

// الحدث الذي يطلقه الهاندلر في حال وجود خطر (للتنبيهات)
public record SecurityAlertRaised(
    string Title,
    string Message,
    AlertLevel Level,
    Guid? RelatedEntityId,
    string Source // "Watchlist" or "GraphAnalysis"
);