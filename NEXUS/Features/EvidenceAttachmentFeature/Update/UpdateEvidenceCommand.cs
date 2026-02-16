namespace NEXUS.Features.EvidenceAttachmentFeature.Update;

public record UpdateEvidenceCommand(
    Guid Id,
    string? Description,
    string? FileName
);
