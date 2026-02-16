using NEXUS.Data.Common;
using System.ComponentModel.DataAnnotations;
namespace NEXUS.Data.Entities;

// تفاصيل البيعة
public class BayahDetail : BaseEntity
{
    public Guid SuspectId { get; set; }
    public Suspect Suspect { get; set; } = default!;

    public DateOnly? Date { get; set; }
    [MaxLength(50)]
    public string? Location { get; set; }
    [MaxLength(60)]
    public string? RecruiterName { get; set; } // المنسِّب
    [MaxLength(500)]
    public string? TextOfPledge { get; set; } // النص
}
