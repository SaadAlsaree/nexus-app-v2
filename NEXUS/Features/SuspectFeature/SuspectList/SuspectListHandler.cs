using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.SuspectFeature.SuspectList;

public class SuspectListHandler
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SuspectListHandler(AppDbContext db, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Response<PagedList<SuspectListDto>>> Handle(
        GetSuspectListQuery query,
        CancellationToken cancellationToken)
    {
        var queryable = _db.Suspects.AsNoTracking().Where(s => !s.IsDeleted);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(s =>
                s.FullName.Contains(query.SearchTerm) ||
                (s.Kunya != null && s.Kunya.Contains(query.SearchTerm)));
        }

        queryable = queryable.OrderByDescending(s => s.CreatedAt);

        var pagedEntities = await PagedList<Suspect>.CreateAsync(
            queryable,
            query.Page ?? 1,
            query.PageSize ?? 10,
            cancellationToken);

        var baseUrl = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}";

        var dtos = _mapper.From(pagedEntities.Items)
                         .AddParameters("BaseUrl", baseUrl)
                         .AdaptToType<List<SuspectListDto>>();

        var result = new PagedList<SuspectListDto>(
            dtos,
            pagedEntities.TotalCount,
            query.Page ?? 1,
            query.PageSize ?? 10);

        return Response<PagedList<SuspectListDto>>.Success(result);
    }
}
