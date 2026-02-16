using NEXUS.Data.Common;
using NEXUS.Data.Enums;
using System.ComponentModel.DataAnnotations;
namespace NEXUS.Data.Entities;
// الدورات التدريبية
public class TrainingCourse : BaseEntity
{
    public Guid SuspectId { get; set; }
    public Suspect Suspect { get; set; } = default!;

    public CourseType CourseType { get; set; } // (شرعية، عسكرية)
    [MaxLength(100)]
    public string? Location { get; set; }
    [MaxLength(100)]
    public string? TrainerName { get; set; } // المدرب (مهم للتقاطعات)
    public string? Classmates { get; set; } // زملاء الدورة
}
