using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;
using NEXUS.Services.Storage;
using System.Security.Cryptography;

namespace NEXUS.Features.EvidenceAttachmentFeature.Create;

public class CreateEvidenceHandler(AppDbContext dbContext)
{
    public async Task<Result<Guid>> Handle(CreateEvidenceCommand command, IBlobService blobService, CancellationToken cancellationToken)
    {
        // Check if Case exists if provided
        if (command.CaseId.HasValue)
        {
            if (!await dbContext.Cases.AnyAsync(c => c.Id == command.CaseId, cancellationToken))
            {
                return Result.Failure<Guid>(Error.NotFound("Case.NotFound", $"Case with ID {command.CaseId} not found."));
            }
        }

        // Check if Suspect exists if provided
        if (command.SuspectId.HasValue)
        {
            if (!await dbContext.Suspects.AnyAsync(s => s.Id == command.SuspectId, cancellationToken))
            {
                return Result.Failure<Guid>(Error.NotFound("Suspect.NotFound", $"Suspect with ID {command.SuspectId} not found."));
            }
        }

        // 1. Calculate MD5 Checksum
        using var md5 = MD5.Create();
        using var stream = command.File.OpenReadStream();
        var hashBytes = await md5.ComputeHashAsync(stream, cancellationToken);
        var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

        // 2. Upload to Storage
        stream.Position = 0; // Reset position
        var uploadResult = await blobService.UploadAsync(stream, command.File.ContentType, cancellationToken);

        if (uploadResult.IsFailure)
        {
            return Result.Failure<Guid>(uploadResult.Error);
        }

        // 3. Save Metadata to DB
        var evidence = new EvidenceAttachment
        {
            Id = Guid.NewGuid(),
            CaseId = command.CaseId,
            SuspectId = command.SuspectId,
            FileName = command.File.FileName,
            FileType = command.File.ContentType,
            FilePath = uploadResult.Value.ToString(),
            HashChecksum = hash,
            Description = command.Description,
        };

        dbContext.EvidenceAttachments.Add(evidence);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(evidence.Id);
    }
}
