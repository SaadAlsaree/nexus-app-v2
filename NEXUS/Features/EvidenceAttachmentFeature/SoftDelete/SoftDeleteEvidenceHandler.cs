using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.EvidenceAttachmentFeature.SoftDelete;

public class SoftDeleteEvidenceHandler(AppDbContext dbContext)
{
    public async Task<Result> Handle(SoftDeleteEvidenceCommand command, CancellationToken cancellationToken)
    {
        var evidence = await dbContext.EvidenceAttachments.FindAsync([command.Id], cancellationToken);

        if (evidence == null)
        {
            return Result.Failure(Error.NotFound("Evidence.NotFound", "The evidence attachment was not found."));
        }

        // Soft delete logic
        evidence.IsDeleted = true;
        evidence.DeletedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
