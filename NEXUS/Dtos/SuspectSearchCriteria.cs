using NEXUS.Data.Enums;

namespace NEXUS.Dtos;

public class SuspectSearchCriteria
{
    // 1. بحث نصي عام (للاسم، الكنية، الملاحظات)
    public string? GeneralQuery { get; set; }

    // 2. فلترة دقيقة (Exact Match filters)
    public string? NationalId { get; set; } // رقم هوية
    public string? Tribe { get; set; } // العشيرة
    public SuspectStatus? Status { get; set; } // الحالة (موقوف، هارب)
    public string? MotherName { get; set; }

    // 3. فلترة النطاقات (Ranges)
    public DateOnly? DateOfBirthFrom { get; set; }
    public DateOnly? DateOfBirthTo { get; set; }

    // 4. البحث في "البيانات الفرعية" (Deep Search)
    public string? PhoneNumber { get; set; } // البحث في جدول Contacts
    public string? AddressCity { get; set; } // البحث في جدول Addresses
    public string? OrgUnitName { get; set; } // البحث في جدول Assignments (كان ينتمي لولاية كذا)

    // 5. البحث الجنائي
    public OperationType? InvolvedInOperationType { get; set; } // شارك في (اغتيال، تفخيخ)

    // 6. الترحيل (Pagination)
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
