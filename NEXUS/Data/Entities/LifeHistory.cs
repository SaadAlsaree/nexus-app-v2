using NEXUS.Data.Common;
using System.ComponentModel.DataAnnotations;
namespace NEXUS.Data.Entities;

// الحياة المدنية السابقة
public class LifeHistory : BaseEntity
{
    public Guid SuspectId { get; set; }
    public Suspect Suspect { get; set; } = default!;

    [MaxLength(50)]
    public string? EducationLevel { get; set; } // مستوى التعليم
    [MaxLength(255)]
    public string? SchoolsAttended { get; set; } // المدارس التي حضرت
    [MaxLength(255)]
    public string? CivilianJobs { get; set; } // الوظائف المدنيه
    public string? RadicalizationStory { get; set; } // قصة الانتماء
}
