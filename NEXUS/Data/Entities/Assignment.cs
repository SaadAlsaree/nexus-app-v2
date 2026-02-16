using NEXUS.Data.Common;
using NEXUS.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS.Data.Entities;

public class Assignment : BaseEntity
{
    public Guid SuspectId { get; set; }
    public Suspect Suspect { get; set; } = default!;

    public Guid OrgUnitId { get; set; }
    public OrgUnit OrgUnit { get; set; } = default!;

    // التسلسل القيادي البشري
    public Guid? DirectCommanderId { get; set; }
    [ForeignKey("DirectCommanderId")]
    public Suspect? DirectCommander { get; set; } // من كان أميره المباشر؟

    public OrgRole RoleTitle { get; set; } // (أمير، جندي، إداري)
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
