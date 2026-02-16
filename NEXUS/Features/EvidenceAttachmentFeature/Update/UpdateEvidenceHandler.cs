using MapsterMapper;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.EvidenceAttachmentFeature.Update;

public class UpdateEvidenceHandler(AppDbContext dbContext, IMapper _mapper)
{
    public async Task<Result> Handle(UpdateEvidenceCommand command, CancellationToken cancellationToken)
    {
        var evidence = await dbContext.EvidenceAttachments
            .FindAsync([command.Id], cancellationToken);

        if (evidence == null || evidence.IsDeleted)
        {
            return Result.Failure(Error.NotFound("Evidence.NotFound", "The evidence attachment was not found."));
        }

        _mapper.Map(command, evidence);
        evidence.LastUpdateAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
