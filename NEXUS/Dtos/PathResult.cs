namespace NEXUS.Dtos;

public class PathResult
{
    // وصف نصي للمسار (سهل القراءة للمحقق)
    // مثال: "Ahmed -> CALLED -> 0770... -> OWNED_BY -> Ali"
    public string PathDescription { get; set; } = string.Empty;

    // عدد القفزات (Hops)
    // كلما قل العدد، كانت العلاقة أقوى وأخطر
    public int Length { get; set; }

    // قائمة العقد في هذا المسار (لغرض رسمها بلون مميز في الواجهة)
    public List<Guid> NodeIdsInPath { get; set; } = new();

    // وزن المسار (اختياري: هل هو مسار مؤكد بأدلة قوية أم استنتاج؟)
    public double ConfidenceScore { get; set; } // 0.0 to 1.0
}