namespace NEXUS.Dtos;

public record SuspectDto(
        Guid Id,
        string FullName,           // الاسم الكامل
        string? Kunya,             // الكنية (مهمة جداً في البحث الأمني)
        string Status,             // الحالة كنص (للعرض المباشر: موقوف، هارب)
        string? CurrentLocation,   // آخر مكان تواجد أو السجن الحالي
        string? MainCaseNumber,    // رقم قضيته الرئيسية (للسهولة)
        int RiskLevel              // مستوى الخطورة (محسوب من النظام)
     );

