using NEXUS.Data.Enums;
using NEXUS.Dtos;

namespace NEXUS.Services;

public interface IIntelligenceEngine
{
    // --- 1. عمليات الغراف الأساسية (كانت سابقاً في GraphService) ---

    // المزامنة: تنشئ أو تحدث أي عقدة (متهم، هاتف، عنوان)
    Task SyncNodeAsync(Guid entityId, string nodeType, Dictionary<string, object> properties);

    // الربط: تنشئ علاقة بين أي عقدتين
    Task CreateLinkAsync(Guid fromId, Guid toId, string relationshipType, Dictionary<string, object>? props = null);

    // الحذف: (اختياري) لحذف علاقة أو عقدة عند الخطأ
    Task DeleteNodeAsync(Guid entityId);

    // --- 2. عمليات التحليل المتقدمة (Intelligence) ---

    // الفحص الأمني عند الإضافة
    Task<AnalysisReportDto> AnalyzeNewEntryAsync(string value, string type);

    // كشف التقاطعات (من ذكر هذه المعلومة أيضاً؟)
    Task<List<SuspectDto>> FindWhoMentionedEntityAsync(string entityValue, Guid excludeSuspectId);

    // البحث عن المسارات (بديل متطور لـ FindConnectionPath)
    Task<List<PathResultDto>> FindBridgesAsync(Guid entityA, Guid entityB);

    Task InferRelationshipsAsync(Guid suspectId);

}

// DTOs مساعدة
public record AnalysisReportDto(
    bool IsFlagged,
    string Message,
    AlertLevel AlertLevel,
    string Source,
    string Recommendation
);

public record PathResultDto(
    string PathDescription,
    int Length,
    List<string> Nodes
);