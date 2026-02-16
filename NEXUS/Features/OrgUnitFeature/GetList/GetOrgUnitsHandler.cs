using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.OrgUnitFeature.GetList;

public sealed class GetOrgUnitsHandler
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public GetOrgUnitsHandler(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<Response<PagedList<OrgUnitSummaryDto>>> Handle(
        GetOrgUnitsQuery query,
        CancellationToken cancellationToken)
    {
        var dbQuery = _db.OrgUnits
            .AsNoTracking()
            .Where(u => !u.IsDeleted);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            dbQuery = dbQuery.Where(u =>
                u.UnitName.Contains(query.SearchTerm) ||
                u.Path.Contains(query.SearchTerm));
        }

        if (query.Level.HasValue)
        {
            dbQuery = dbQuery.Where(u => u.Level == query.Level.Value);
        }

        dbQuery = dbQuery.OrderBy(u => u.Path);

        var pagedEntities = await PagedList<OrgUnit>.CreateAsync(
            dbQuery,
            query.Page,
            query.PageSize,
            cancellationToken);

        var dtos = _mapper.From(pagedEntities.Items)
                         .AdaptToType<List<OrgUnitSummaryDto>>();

        var result = new PagedList<OrgUnitSummaryDto>(
            dtos,
            pagedEntities.TotalCount,
            query.Page,
            query.PageSize);

        return Response<PagedList<OrgUnitSummaryDto>>.Success(result);
    }
}
