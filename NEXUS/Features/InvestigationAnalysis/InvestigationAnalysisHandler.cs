using NEXUS.Data;
using NEXUS.Data.Enums;
using NEXUS.Events;
using NEXUS.Services;
using NEXUS.Services.TextAnalysis;
using System.Security.Cryptography; // ضروري للـ MD5
using System.Text;

namespace NEXUS.Features.InvestigationAnalysis;

public class InvestigationAnalysisHandler
{
    private readonly ITextAnalysisService _analyzer;
    private readonly IIntelligenceEngine _intelligenceEngine;
    private readonly AppDbContext _sqlContext;
    private readonly ILogger<InvestigationAnalysisHandler> _logger;

    public InvestigationAnalysisHandler(
        ITextAnalysisService analyzer,
        IIntelligenceEngine intelligenceEngine,
        AppDbContext sqlContext,
        ILogger<InvestigationAnalysisHandler> logger)
    {
        _analyzer = analyzer;
        _intelligenceEngine = intelligenceEngine;
        _sqlContext = sqlContext;
        _logger = logger;
    }

    public async Task<BackgroundJobs.SecurityAlertRaised?> Handle(InterrogationSessionCreatedEvent @event)
    {
        // 1. تحليل النص
        var analysis = await _analyzer.AnalyzeTextAsync(@event.Content);

        // 2. تحديث الملخص في SQL (تم تفعيله الآن)
        if (string.IsNullOrEmpty(@event.SummaryContent) && !string.IsNullOrEmpty(analysis.GeneratedSummary))
        {
            try
            {
                var session = await _sqlContext.InterrogationSessions.FindAsync(@event.SessionId);
                if (session != null)
                {
                    session.SummaryContent = analysis.GeneratedSummary;
                    await _sqlContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating session summary via SQL.");
            }
        }

        List<string> intersectionWarnings = new();

        // 3. معالجة الكيانات
        foreach (var entity in analysis.Entities)
        {
            // تنظيف النص قبل توليد الـ ID لضمان التطابق (إزالة مسافات وتوحيد الأحرف)
            string cleanValue = entity.Value.Trim().ToLowerInvariant();

            // توليد ID ثابت بناءً على القيمة
            var entityNodeId = GenerateDeterministicGuid(cleanValue);

            // أ. مزامنة العقدة (تم التصحيح هنا: استخدام entityNodeId بدلاً من NewGuid)
            await _intelligenceEngine.SyncNodeAsync(
                entityNodeId,
                entity.Type,
                new Dictionary<string, object> { { "value", entity.Value } }
            );

            // ب. ربط المتهم بالعقدة
            await _intelligenceEngine.CreateLinkAsync(@event.SuspectId, entityNodeId, "MENTIONED");

            // ج. كشف التقاطعات
            // نبحث عن أي شخص آخر ذكر نفس القيمة (Clean Value)
            var intersections = await _intelligenceEngine.FindWhoMentionedEntityAsync(entity.Value, @event.SuspectId);

            if (intersections.Any())
            {
                foreach (var otherSuspect in intersections)
                {
                    intersectionWarnings.Add($"الكيان '{entity.Value}' ({entity.Type}) تم ذكره أيضاً بواسطة {otherSuspect.FullName} (قضية {otherSuspect.MainCaseNumber})");
                }
            }
        }

        // 4. إطلاق التنبيه إذا وجد خطر
        if (intersectionWarnings.Any())
        {
            return new BackgroundJobs.SecurityAlertRaised(
                Title: "اكتشاف تقاطع في الاعترافات",
                Message: string.Join("\n", intersectionWarnings),
                Level: AlertLevel.High,
                RelatedEntityId: @event.SuspectId,
                Source: "InvestigationAnalysis"
            );
        }

        return null;
    }

    // دالة توليد GUID ثابت (Deterministic)
    private Guid GenerateDeterministicGuid(string input)
    {
        // استخدام MD5 لإنشاء Hash ثابت من النص
        using (var md5 = MD5.Create())
        {
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return new Guid(hash);
        }
    }
}