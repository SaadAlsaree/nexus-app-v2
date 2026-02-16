using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.EntityWatchlistFeature.Delete;

public sealed class SoftDeleteWatchlistEntryHandler(AppDbContext _db)
{
    public async Task<Response<WatchlistEntryDeletedEvent>> Handle(SoftDeleteWatchlistEntryCommand command)
    {
        var entry = await _db.EntityWatchlists.FindAsync(command.Id);
        if (entry == null)
            return Response<WatchlistEntryDeletedEvent>.Failure(Error.NotFound("Watchlist.NotFound", "Watchlist entry not found."));

        entry.IsDeleted = true;
        entry.DeletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<WatchlistEntryDeletedEvent>.Success(new WatchlistEntryDeletedEvent(entry.Id));
    }
}
