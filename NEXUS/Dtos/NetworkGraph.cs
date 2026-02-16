namespace NEXUS.Dtos;

public class NetworkGraph
{
    // الدوائر (الأشخاص، الهواتف، الأماكن)
    public List<GraphNode> Nodes { get; set; } = new();

    // الخطوط (العلاقات بينهم)
    public List<GraphLink> Links { get; set; } = new();
}

// تفاصيل العقدة الواحدة
public class GraphNode
{
    public string Id { get; set; } = string.Empty; // Guid as string
    public string Label { get; set; } = string.Empty; // "Suspect", "Phone"
    public string DisplayName { get; set; } = string.Empty; // "Abu Musab"
    public string Color { get; set; } = string.Empty; // أحمر للمحكومين، أصفر للمشتبه بهم
    public object Metadata { get; set; } = string.Empty; // بيانات إضافية عند النقر
}

// تفاصيل الرابط الواحد
public class GraphLink
{
    public string SourceId { get; set; } = string.Empty; // من
    public string TargetId { get; set; } = string.Empty;     // إلى
    public string Type { get; set; } = string.Empty;// "BROTHER_OF", "CALLED"
    public string Label { get; set; } = string.Empty;// النص الذي يظهر على الخط
}