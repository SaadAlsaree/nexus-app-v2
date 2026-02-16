namespace NEXUS.Features.SuspectFeature.GetNetwork;

public record GetSuspectNetworkQuery(
    Guid SuspectId,
    int Depth = 4,             // افتراضياً 1 (المباشرين فقط)، يمكن زيادته لـ 2 أو 3
    bool IncludeFamilyOnly = false  // (اختياري) فلتر إضافي);
    );
