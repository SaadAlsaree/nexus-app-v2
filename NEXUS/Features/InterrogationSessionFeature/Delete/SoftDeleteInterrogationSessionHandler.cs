using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.InterrogationSessionFeature.Delete;

public sealed class SoftDeleteInterrogationSessionHandler(AppDbContext _db)
{
    public async Task<Response<InterrogationSessionDeletedEvent>> Handle(SoftDeleteInterrogationSessionCommand command)
    {
        var session = await _db.InterrogationSessions.FindAsync(command.Id);
        if (session == null)
            return Response<InterrogationSessionDeletedEvent>.Failure(Error.NotFound("InterrogationSession.NotFound", "Interrogation session not found."));

        session.IsDeleted = true;
        session.DeletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<InterrogationSessionDeletedEvent>.Success(new InterrogationSessionDeletedEvent(session.Id));
    }
}
