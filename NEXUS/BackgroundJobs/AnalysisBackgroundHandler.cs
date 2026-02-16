using NEXUS.Services;

namespace NEXUS.BackgroundJobs;

public class AnalysisBackgroundHandler
{
    private readonly IIntelligenceEngine _intelligenceEngine;
    private readonly ILogger<AnalysisBackgroundHandler> _logger;

    public AnalysisBackgroundHandler(
        IIntelligenceEngine intelligenceEngine,
        ILogger<AnalysisBackgroundHandler> logger)
    {
        _intelligenceEngine = intelligenceEngine;
        _logger = logger;
    }

    // Wolverine يستدعي هذه الدالة تلقائياً عند نشر ContactAddedEvent
    // القيمة المرجعة (SecurityAlertRaised?) هي رسالة متتالية (Cascading Message)
    public async Task<SecurityAlertRaised?> Handle(ContactAddedEvent @event)
    {
        _logger.LogInformation($"Starting intelligence analysis for contact: {@event.PhoneNumber}");

        // ---------------------------------------------------------
        // 1. المزامنة مع Neo4j (Sync Phase)
        // ---------------------------------------------------------
        try
        {
            // أ. إنشاء عقدة الهاتف (أو جلبها إن وجدت)
            await _intelligenceEngine.SyncNodeAsync(
                @event.ContactId,
                "Phone",
                new Dictionary<string, object>
                {
                        { "value", @event.PhoneNumber },
                        { "type", @event.ContactType }
                }
            );

            // ب. إنشاء الرابط بين المتهم والهاتف (Suspect -[HAS_CONTACT]-> Phone)
            await _intelligenceEngine.CreateLinkAsync(
                @event.SuspectId,
                @event.ContactId,
                "HAS_CONTACT"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to sync contact to Neo4j.");
            // لا نوقف التنفيذ، نكمل للتحليل الأمني فهو أهم
        }

        // ---------------------------------------------------------
        // 2. التحليل الأمني (Analysis Phase)
        // ---------------------------------------------------------

        // نفحص الرقم: هل هو في قائمة المراقبة؟ أو مرتبط بشبكة خطرة؟
        var analysisReport = await _intelligenceEngine.AnalyzeNewEntryAsync(
            @event.PhoneNumber,
            "Phone"
        );

        // ---------------------------------------------------------
        // 3. اتخاذ القرار (Decision Phase)
        // ---------------------------------------------------------

        if (analysisReport.IsFlagged)
        {
            _logger.LogWarning($"SECURITY ALERT: {@event.PhoneNumber} - {analysisReport.Message}");

            // إرجاع حدث جديد! 
            // Wolverine سيقوم بنشر هذا الحدث تلقائياً ليلتقطه هاندلر التنبيهات (NotificationHandler)
            return new SecurityAlertRaised(
                Title: "كشف تطابق أمني خطير",
                Message: $"تم رصد رقم هاتف ({@event.PhoneNumber}) مطابق لقائمة المراقبة. التفاصيل: {analysisReport.Message}",
                Level: analysisReport.AlertLevel,
                RelatedEntityId: @event.SuspectId,
                Source: analysisReport.Source
            );
        }

        // إذا لم يكن هناك خطر، نرجع null (لا شيء يحدث)
        return null;
    }
}
