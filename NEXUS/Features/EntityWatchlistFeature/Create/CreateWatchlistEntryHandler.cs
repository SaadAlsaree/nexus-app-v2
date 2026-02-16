using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.EntityWatchlistFeature.Create;

public sealed class CreateWatchlistEntryHandler(AppDbContext _db)
{
    public async Task<(Response<Guid>, WatchlistEntryCreatedEvent)> Handle(CreateWatchlistEntryCommand command)
    {
        var entry = new EntityWatchlist
        {
            Id = Guid.NewGuid(),
            Keyword = command.Keyword,
            AlertLevel = command.AlertLevel,
            Reason = command.Reason,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _db.EntityWatchlists.Add(entry);
        await _db.SaveChangesAsync();

        return (Response<Guid>.Success(entry.Id), new WatchlistEntryCreatedEvent(entry.Id, entry.Keyword));
    }
}
