using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.EntityWatchlistFeature.GetList;

public sealed class GetWatchlistHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<List<WatchlistEntryDto>>> Handle(GetWatchlistQuery query)
    {
        var dbQuery = _db.EntityWatchlists
            .AsNoTracking()
            .Where(w => !w.IsDeleted);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            dbQuery = dbQuery.Where(w => w.Keyword.Contains(query.SearchTerm) || w.Reason.Contains(query.SearchTerm));
        }

        var entries = await dbQuery
            .OrderByDescending(w => w.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var dtos = _mapper.Map<List<WatchlistEntryDto>>(entries);

        return Response<List<WatchlistEntryDto>>.Success(dtos);
    }
}
