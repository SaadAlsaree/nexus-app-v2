using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.CaseFeature.RemoveSupectFromCase;

public sealed class RemoveSuspectFromCaseHandler(AppDbContext _db)
{
    public async Task<Response<SuspectRemovedFromCaseEvent>> Handle(RemoveSuspectFromCaseCommand command)
    {
        var caseSuspect = await _db.CaseSuspects
            .FirstOrDefaultAsync(cs => cs.CaseId == command.CaseId && cs.SuspectId == command.SuspectId);

        if (caseSuspect == null)
            return Response<SuspectRemovedFromCaseEvent>.Failure(Error.NotFound("CaseSuspect.NotFound", "Suspect is not linked to this case."));

        var @case = await _db.Cases.FindAsync(command.CaseId);
        if (@case != null && @case.SuspectsCount > 0)
        {
            @case.SuspectsCount--;
        }

        _db.CaseSuspects.Remove(caseSuspect);
        await _db.SaveChangesAsync();

        return Response<SuspectRemovedFromCaseEvent>.Success(new SuspectRemovedFromCaseEvent(command.CaseId, command.SuspectId));
    }
}
