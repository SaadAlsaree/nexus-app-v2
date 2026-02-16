using NEXUS.Data.Common;
using NEXUS.Data.Enums;

namespace NEXUS.Data.Entities;

public class CaseSuspect : BaseEntity
{
    public Guid CaseId { get; set; }
    public Case Case { get; set; } = default!;

    public Guid SuspectId { get; set; }
    public Suspect Suspect { get; set; } = default!;

    public AccusationType AccusationType { get; set; } // نوع التهمة في هذه القضية
    public LegalStatusInCase LegalStatus { get; set; } // (موقوف، مكفول)
    public DateOnly? ConfessionDate { get; set; } // تاريخ التهمة
    public string? Notes { get; set; }
}
