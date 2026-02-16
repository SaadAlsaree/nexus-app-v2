using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.InterrogationSessionFeature.GetList;

public class GetListInterrogationSessionsHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<PagedList<InterrogationSessionSummaryDto>>> Handle(
        GetListInterrogationSessionsQuery query,
        CancellationToken cancellationToken)
    {
        var queryable = _db.InterrogationSessions
            .AsNoTracking()
            .Include(s => s.Suspect)
            .Include(s => s.Case)
            .Where(s => !s.IsDeleted);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(s =>
                s.InterrogatorName.Contains(query.SearchTerm) ||
                s.Location.Contains(query.SearchTerm) ||
                s.Suspect.FirstName.Contains(query.SearchTerm) ||
                s.Suspect.SecondName.Contains(query.SearchTerm) ||
                s.Case.Title.Contains(query.SearchTerm));
        }

        queryable = queryable.OrderByDescending(s => s.SessionDate);

        var pagedEntities = await PagedList<InterrogationSession>.CreateAsync(
            queryable,
            query.Page,
            query.PageSize,
            cancellationToken);

        var dtos = _mapper.From(pagedEntities.Items)
                         .AdaptToType<List<InterrogationSessionSummaryDto>>();

        var result = new PagedList<InterrogationSessionSummaryDto>(
            dtos,
            pagedEntities.TotalCount,
            query.Page,
            query.PageSize);

        return Response<PagedList<InterrogationSessionSummaryDto>>.Success(result);
    }
}
