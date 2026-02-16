using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.CaseFeature.SoftDeleteCase;

public sealed class SoftDeleteCaseHandler(AppDbContext _db)
{
    public async Task<Response<CaseDeletedEvent>> Handle(SoftDeleteCaseCommand command)
    {
        var @case = await _db.Cases.FindAsync(command.CaseId);
        if (@case == null)
            return Response<CaseDeletedEvent>.Failure(Error.NotFound("Case.NotFound", "Case not found."));

        @case.IsDeleted = true;
        @case.DeletedAt = DateTime.UtcNow;
        // Optionally add DeletedBy if available in context

        await _db.SaveChangesAsync();

        return Response<CaseDeletedEvent>.Success(new CaseDeletedEvent(@case.Id));
    }
}
