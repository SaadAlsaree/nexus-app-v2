using NEXUS.Data.Common;
using NEXUS.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS.Data.Entities;

public class OrgUnit : BaseEntity
{
    public string UnitName { get; set; } = string.Empty; // (ولاية الجنوب، قاطع الرشيد)
    public OrgUnitLevel Level { get; set; } // (ولاية، قاطع، مفرزة)

    // --- Self-Referencing (الهرمية) ---
    public Guid? ParentUnitId { get; set; }
    [ForeignKey("ParentUnitId")]
    public OrgUnit? ParentUnit { get; set; } // الأب
    [MaxLength(255)]
    public string Path { get; set; } = string.Empty; // (ولاية الجنوب -> قاطع الرشيد -> مفرزة)

    public ICollection<OrgUnit> SubUnits { get; set; } = default!; // الأبناء

    // الموظفين في هذه الوحدة
    public ICollection<Assignment> Assignments { get; set; } = default!;
}
