using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NEXUS.Features.EvidenceAttachmentFeature.Create;

public record CreateEvidenceCommand(
    IFormFile File,
    Guid? CaseId,
    Guid? SuspectId,
    string? Description
);
