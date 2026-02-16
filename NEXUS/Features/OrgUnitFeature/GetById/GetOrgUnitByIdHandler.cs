using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.OrgUnitFeature.GetById;

public sealed class GetOrgUnitByIdHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<OrgUnitDetailsDto>> Handle(GetOrgUnitByIdQuery query)
    {
        var unit = await _db.OrgUnits
            .AsNoTracking()
            .Include(u => u.ParentUnit)
            .Where(u => u.Id == query.Id && !u.IsDeleted)
            .FirstOrDefaultAsync();

        if (unit == null)
            return Response<OrgUnitDetailsDto>.Failure(Error.NotFound("OrgUnit.NotFound", "Organizational unit not found."));

        var dto = _mapper.Map<OrgUnitDetailsDto>(unit);

        return Response<OrgUnitDetailsDto>.Success(dto);
    }
}
