using NEXUS.Data.Common;
using NEXUS.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS.Data.Entities;

public class Address : BaseEntity
{
    public Guid SuspectId { get; set; }
    [ForeignKey("SuspectId")]
    public Suspect Suspect { get; set; } = default!;

    public AddressType Type { get; set; } // (سكن أصلي، مضافة)
    [MaxLength(25)]
    public string Country { get; set; } = string.Empty;
    [MaxLength(25)]
    public string City { get; set; } = string.Empty;
    [MaxLength(25)]
    public string? District { get; set; } // (حي)
    [MaxLength(500)]
    public string? Details { get; set; } // (تفاصيل)
    [MaxLength(50)]
    public string? GPSCoordinates { get; set; } // (إحداثيات)
}

