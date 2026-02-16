using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.CaseFeature.GetByIdCase;

public class GetByIdCaseHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<CaseDetailsDto>> Handle(GetByIdCaseQuery query)
    {
        var @case = await _db.Cases
            .AsNoTracking()
            .Include(c => c.SuspectsInvolved)
                .ThenInclude(cs => cs.Suspect)
            .FirstOrDefaultAsync(c => c.Id == query.Id && !c.IsDeleted);

        if (@case == null)
            return Response<CaseDetailsDto>.Failure(Error.NotFound("Case.NotFound", "Case not found."));

        var dto = _mapper.Map<CaseDetailsDto>(@case);

        return Response<CaseDetailsDto>.Success(dto);
    }
}
