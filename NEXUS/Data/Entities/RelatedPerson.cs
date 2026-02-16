using NEXUS.Data.Common;
using NEXUS.Data.Enums;
using System.ComponentModel.DataAnnotations;
namespace NEXUS.Data.Entities;

public class RelatedPerson : BaseEntity
{
    public Guid SuspectId { get; set; }
    public Suspect Suspect { get; set; } = default!;

    [MaxLength(25)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(25)]
    public string SecondName { get; set; } = string.Empty;
    [MaxLength(25)]
    public string ThirdName { get; set; } = string.Empty;
    [MaxLength(25)]
    public string? FourthName { get; set; }
    [MaxLength(25)]
    public string? FivthName { get; set; }
    [MaxLength(25)]
    public string? Tribe { get; set; }

    [MaxLength(255)]
    public string FullName { get; set; } = string.Empty;
    public RelationshipType Relationship { get; set; } // (أخ، خال، نسيب)
    [MaxLength(255)]
    public string? SpouseName { get; set; } // للبحث عن المصاهرة
    [MaxLength(255)]
    public string? CurrentStatus { get; set; } // (موقوف، مدني)
    [MaxLength(500)]
    public string? Notes { get; set; }

    public void UpdateFullName()
    {
        FullName = string.Join(" ", new[] { FirstName, SecondName, ThirdName, FourthName, FivthName, Tribe }
            .Where(s => !string.IsNullOrWhiteSpace(s))).Trim();
    }
}
