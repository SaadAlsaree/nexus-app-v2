namespace NEXUS.Dtos;

public record ExtractedEntity(
    string Value,        // القيمة (مثلاً: 0770123456)
    string Type,         // النوع (Phone, Person, Location, Weapon)
    string Context,      // السياق (مثلاً: "اتصلت به لنقل السلاح")
    double Confidence    // نسبة الثقة (0.0 - 1.0)
);

public record AnalysisResult(
    List<ExtractedEntity> Entities,
    string GeneratedSummary // ملخص آلي للافادة
);