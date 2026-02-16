using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Features.EvidenceAttachmentFeature.GetList;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.EvidenceAttachmentFeature.GetById;

public class GetEvidenceByIdHandler(AppDbContext dbContext, IMapper _mapper)
{
    public async Task<Result<EvidenceDto>> Handle(
        GetEvidenceByIdQuery query,
        [FromServices] IHttpContextAccessor contextAccessor,
        CancellationToken cancellationToken)
    {
        var evidence = await dbContext.EvidenceAttachments
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == query.Id && !e.IsDeleted, cancellationToken);

        if (evidence == null)
        {
            return Result.Failure<EvidenceDto>(Error.NotFound("Evidence.NotFound", "The evidence attachment was not found."));
        }

        var baseUrl = $"{contextAccessor.HttpContext?.Request.Scheme}://{contextAccessor.HttpContext?.Request.Host}/api/storage/";

        var dto = _mapper.From(evidence)
            .AddParameters("BaseUrl", baseUrl)
            .AdaptToType<EvidenceDto>();

        return Result.Success(dto);
    }
}
