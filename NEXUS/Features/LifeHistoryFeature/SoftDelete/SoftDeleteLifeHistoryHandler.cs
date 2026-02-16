using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.LifeHistoryFeature.SoftDelete;

public sealed class SoftDeleteLifeHistoryHandler(AppDbContext _db)
{
    public async Task<Response<LifeHistoryDeletedEvent>> Handle(SoftDeleteLifeHistoryCommand command)
    {
        var lifeHistory = await _db.LifeHistories.FindAsync(command.LifeHistoryId);
        if (lifeHistory == null)
            return Response<LifeHistoryDeletedEvent>.Failure(Error.NotFound("LifeHistory.NotFound", "Life history record not found."));

        lifeHistory.IsDeleted = true;
        lifeHistory.DeletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<LifeHistoryDeletedEvent>.Success(new LifeHistoryDeletedEvent(lifeHistory.Id));
    }
}
