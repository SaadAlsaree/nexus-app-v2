using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Features.EvidenceAttachmentFeature.GetList;

namespace NEXUS.Features.EvidenceAttachmentFeature.GetBySuspectId;

public class GetEvidenceBySuspectIdHandler(AppDbContext dbContext, IMapper _mapper)
{
    public async Task<IEnumerable<EvidenceDto>> Handle(
        GetEvidenceBySuspectIdQuery query,
        [FromServices] IHttpContextAccessor contextAccessor,
        CancellationToken cancellationToken)
    {
        var list = await dbContext.EvidenceAttachments
            .AsNoTracking()
            .Where(e => e.SuspectId == query.SuspectId && !e.IsDeleted)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);

        var baseUrl = $"{contextAccessor.HttpContext?.Request.Scheme}://{contextAccessor.HttpContext?.Request.Host}/api/storage/";

        var dtos = _mapper.From(list)
            .AddParameters("BaseUrl", baseUrl)
            .AdaptToType<IEnumerable<EvidenceDto>>();

        return dtos;
    }
}
