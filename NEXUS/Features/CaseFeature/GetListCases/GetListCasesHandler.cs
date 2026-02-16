using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.CaseFeature.GetListCases;

public class GetListCasesHandler
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public GetListCasesHandler(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<Response<PagedList<CaseSummaryDto>>> Handle(
        GetListCasesQuery query,
        CancellationToken cancellationToken)
    {
        var queryable = _db.Cases
            .AsNoTracking()
            .Where(c => !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(c =>
                c.CaseFileNumber.Contains(query.SearchTerm) ||
                c.Title.Contains(query.SearchTerm) ||
                (c.InvestigatingOfficer != null && c.InvestigatingOfficer.Contains(query.SearchTerm)));
        }

        queryable = queryable.OrderByDescending(c => c.CreatedAt);

        var pagedEntities = await PagedList<Case>.CreateAsync(
            queryable,
            query.Page ?? 1,
            query.PageSize ?? 10,
            cancellationToken);

        var dtos = _mapper.From(pagedEntities.Items)
                         .AdaptToType<List<CaseSummaryDto>>();

        var result = new PagedList<CaseSummaryDto>(
            dtos,
            pagedEntities.TotalCount,
            query.Page ?? 1,
            query.PageSize ?? 10);

        return Response<PagedList<CaseSummaryDto>>.Success(result);
    }
}
