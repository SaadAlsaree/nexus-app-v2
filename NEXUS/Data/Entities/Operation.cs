using NEXUS.Data.Common;
using NEXUS.Data.Enums;
using System.ComponentModel.DataAnnotations;
namespace NEXUS.Data.Entities;

// العمليات والمعارك
public class Operation : BaseEntity
{
    public Guid SuspectId { get; set; }
    public Suspect Suspect { get; set; } = default!;

    public OperationType OperationType { get; set; } // (هجوم، تفخيخ)
    public DateOnly? Date { get; set; } // تاريخ العملية
    [MaxLength(255)]
    public string? Location { get; set; }
    [MaxLength(255)]
    public string? RoleInOp { get; set; }
    [MaxLength(255)]
    public string? Associates { get; set; } // شركاء الجريمة
}
