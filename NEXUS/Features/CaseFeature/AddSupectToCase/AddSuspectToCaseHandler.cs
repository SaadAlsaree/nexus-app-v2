using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace NEXUS.Features.CaseFeature.AddSupectToCase;

public sealed class AddSuspectToCaseHandler(AppDbContext _db)
{
    public async Task<Response<SuspectAddedToCaseEvent>> Handle(AddSuspectToCaseCommand command)
    {
        // Check if already exists
        var exists = await _db.CaseSuspects.AnyAsync(cs =>
            cs.CaseId == command.CaseId && cs.SuspectId == command.SuspectId);

        if (exists)
            return Response<SuspectAddedToCaseEvent>.Failure(Error.Conflict("CaseSuspect.Exists", "Suspect is already linked to this case."));

        var caseSuspect = new CaseSuspect
        {
            Id = Guid.NewGuid(),
            CaseId = command.CaseId,
            SuspectId = command.SuspectId,
            AccusationType = command.AccusationType,
            LegalStatus = command.LegalStatus,
            ConfessionDate = command.ConfessionDate,
            Notes = command.Notes,
            CreatedAt = DateTime.UtcNow
        };

        var @case = await _db.Cases.FindAsync(command.CaseId);
        if (@case != null)
        {
            @case.SuspectsCount++;
        }

        _db.CaseSuspects.Add(caseSuspect);
        await _db.SaveChangesAsync();

        return Response<SuspectAddedToCaseEvent>.Success(new SuspectAddedToCaseEvent(command.CaseId, command.SuspectId));
    }
}
