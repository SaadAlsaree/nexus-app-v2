namespace NEXUS.Dtos;

public class SuspectSearchResultDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Kunya { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;// نصي للعرض
    public string CurrentLocation { get; set; } = string.Empty; // آخر عنوان معروف
    public int RiskLevel { get; set; } // يمكن حسابه لاحقاً
    public string LastRole { get; set; } = string.Empty;// آخر منصب تنظيمي
    public int CaseCount { get; set; } // عدد القضايا المتورط بها
}