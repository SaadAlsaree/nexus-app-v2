using NEXUS.Data.Common;
using NEXUS.Data.Enums;
using System.ComponentModel.DataAnnotations;
namespace NEXUS.Data.Entities;

public class Contact : BaseEntity
{
    public Guid SuspectId { get; set; }
    public Suspect Suspect { get; set; } = default!;

    public ContactType Type { get; set; } // (هاتف، تليغرام)
    [MaxLength(50)]
    public string Value { get; set; } = default!; // الرقم أو المعرف
    [MaxLength(50)]
    public string? OwnerName { get; set; } // لمن يعود الرقم
}
