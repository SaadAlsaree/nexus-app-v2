using MapsterMapper;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.EntityWatchlistFeature.Update;

public sealed class UpdateWatchlistEntryHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<WatchlistEntryUpdatedEvent>> Handle(UpdateWatchlistEntryCommand command)
    {
        var entry = await _db.EntityWatchlists.FindAsync(command.Id);
        if (entry == null)
            return Response<WatchlistEntryUpdatedEvent>.Failure(Error.NotFound("Watchlist.NotFound", "Watchlist entry not found."));

        _mapper.Map(command, entry);
        entry.LastUpdateAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<WatchlistEntryUpdatedEvent>.Success(new WatchlistEntryUpdatedEvent(entry.Id));
    }
}
