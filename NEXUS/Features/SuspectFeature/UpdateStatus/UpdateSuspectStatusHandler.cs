using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.SuspectFeature.UpdateStatus;

public sealed class UpdateSuspectStatusHandler(AppDbContext db)
{
    public async Task<Response<SuspectStatusUpdatedEvent>> Handle(UpdateSuspectStatusCommand command)
    {
        var suspect = await db.Suspects.FindAsync(command.SuspectId);
        if (suspect == null) throw new ArgumentException("Suspect not found");

        var oldStatus = suspect.CurrentStatus;
        suspect.CurrentStatus = command.NewStatus;

        // Log history if needed, for now just update
        // We could add a record to LifeHistory here as well

        await db.SaveChangesAsync();

        return Response<SuspectStatusUpdatedEvent>.Success(new SuspectStatusUpdatedEvent(command.SuspectId, oldStatus, command.NewStatus));
    }
}
