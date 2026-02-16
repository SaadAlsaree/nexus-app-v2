namespace NEXUS.Features.EvidenceAttachmentFeature.GetList;

public record EvidenceDto(
    Guid Id,
    string FileName,
    string FileType,
    string HashChecksum,
    string? Description,
    DateTime CreatedAt,
    string DownloadUrl
);
