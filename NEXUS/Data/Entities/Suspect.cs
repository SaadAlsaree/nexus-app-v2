using NEXUS.Data.Common;
using NEXUS.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace NEXUS.Data.Entities;

public class Suspect : BaseEntity
{
    [MaxLength(25)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(25)]
    public string SecondName { get; set; } = string.Empty;
    [MaxLength(25)]
    public string ThirdName { get; set; } = string.Empty;
    [MaxLength(25)]
    public string? FourthName { get; set; }
    [MaxLength(25)]
    public string? FivthName { get; set; } = string.Empty;
    [Required, MaxLength(255)]
    public string FullName { get; set; } = string.Empty;  // الاسم الكامل

    [MaxLength(100)]
    public string? CodeNum { get; set; }

    [MaxLength(100)]
    public string? Kunya { get; set; } // الكنية (أبو فلان)

    [MaxLength(100)]
    public string? MotherName { get; set; }

    public DateOnly? DateOfBirth { get; set; }
    [MaxLength(255)]
    public string? PlaceOfBirth { get; set; }
    [MaxLength(40)]
    public string? Tribe { get; set; } // العشيرة
    public MaritalStatus? MaritalStatus { get; set; }
    public int WivesCount { get; set; }
    public int ChildrenCount { get; set; }

    [MaxLength(100)]
    public string? LegalArticle { get; set; } // المادة القانونية (4 إرهاب)
    [MaxLength(100)]
    public string? HealthStatus { get; set; }
    [MaxLength(250)]
    public string? PhotoUrl { get; set; }

    [Required]
    public SuspectStatus CurrentStatus { get; set; } // (موقوف، هارب، مقتول، محكوم)

    // --- Navigation Properties (العلاقات) ---
    public ICollection<Address> Addresses { get; set; } = default!;
    public ICollection<Contact> Contacts { get; set; } = default!;
    public ICollection<RelatedPerson> RelativesAndAssociates { get; set; } = default!;
    public ICollection<LifeHistory> LifeHistories { get; set; } = default!;

    // العلاقات التنظيمية
    public ICollection<BayahDetail> BayahDetails { get; set; } = default!;
    public ICollection<TrainingCourse> TrainingCourses { get; set; } = default!;
    public ICollection<Operation> Operations { get; set; } = default!;
    public ICollection<Assignment> OrganizationalAssignments { get; set; } = default!; // المناصب التي شغلها

    // القضايا
    public ICollection<CaseSuspect> CaseInvolvements { get; set; } = default!;

    public void UpdateFullName()
    {
        FullName = string.Join(" ", new[] { FirstName, SecondName, ThirdName, FourthName, FivthName, Tribe }
            .Where(s => !string.IsNullOrWhiteSpace(s))).Trim();
    }
}
