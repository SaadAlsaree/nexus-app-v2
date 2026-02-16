using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.EvidenceAttachmentFeature.GetList;

public class GetEvidenceListHandler(AppDbContext dbContext, IMapper _mapper)
{
    public async Task<IEnumerable<EvidenceDto>> Handle(
        GetEvidenceListQuery query,
        [FromServices] IHttpContextAccessor contextAccessor,
        CancellationToken cancellationToken)
    {
        var dbQuery = dbContext.EvidenceAttachments
            .AsNoTracking()
            .Where(e => !e.IsDeleted)
            .AsQueryable();

        if (query.CaseId.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.CaseId == query.CaseId);
        }

        if (query.SuspectId.HasValue)
        {
            dbQuery = dbQuery.Where(e => e.SuspectId == query.SuspectId);
        }

        var list = await dbQuery
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);

        var baseUrl = $"{contextAccessor.HttpContext?.Request.Scheme}://{contextAccessor.HttpContext?.Request.Host}/api/storage/";

        var dtos = _mapper.From(list)
            .AddParameters("BaseUrl", baseUrl)
            .AdaptToType<IEnumerable<EvidenceDto>>();

        return dtos;
    }
}
