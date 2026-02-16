using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Features.EntityWatchlistFeature.GetList;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.EntityWatchlistFeature.GetById;

public sealed class GetWatchlistEntryByIdHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<WatchlistEntryDto>> Handle(GetWatchlistEntryByIdQuery query)
    {
        var entry = await _db.EntityWatchlists
            .AsNoTracking()
            .Where(w => w.Id == query.Id && !w.IsDeleted)
            .FirstOrDefaultAsync();

        if (entry == null)
            return Response<WatchlistEntryDto>.Failure(Error.NotFound("Watchlist.NotFound", "Watchlist entry not found."));

        var dto = _mapper.Map<WatchlistEntryDto>(entry);

        return Response<WatchlistEntryDto>.Success(dto);
    }
}
