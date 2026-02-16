using NEXUS.Data.Common;
using System.ComponentModel.DataAnnotations;
namespace NEXUS.Data.Entities;

public class EvidenceAttachment : BaseEntity
{
    public Guid? CaseId { get; set; }
    public Case? Case { get; set; }

    public Guid? SuspectId { get; set; }
    public Suspect? Suspect { get; set; }

    [MaxLength(25)]
    public string FileType { get; set; } = string.Empty; // (Audio, PDF)
    [MaxLength(255)]
    public string FilePath { get; set; } = string.Empty;
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;
    [MaxLength(50)]
    public string HashChecksum { get; set; } = string.Empty; // MD5 للحماية من التلاعب
    [MaxLength(500)]
    public string? Description { get; set; }
}
