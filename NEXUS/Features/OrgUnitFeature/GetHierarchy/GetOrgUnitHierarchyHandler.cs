using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.OrgUnitFeature.GetHierarchy;

public sealed class GetOrgUnitHierarchyHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<List<OrgUnitHierarchyDto>>> Handle(GetOrgUnitHierarchyQuery query)
    {
        var allUnits = await _db.OrgUnits
            .AsNoTracking()
            .Where(u => !u.IsDeleted)
            .ToListAsync();

        var lookup = allUnits.ToLookup(u => u.ParentUnitId);

        List<OrgUnitHierarchyDto> BuildHierarchy(Guid? parentId)
        {
            return lookup[parentId]
                .Select(u => {
                    var dto = _mapper.Map<OrgUnitHierarchyDto>(u);
                    var subUnits = BuildHierarchy(u.Id);
                    return dto with { SubUnits = subUnits };
                })
                .ToList();
        }

        var hierarchy = BuildHierarchy(null);

        return Response<List<OrgUnitHierarchyDto>>.Success(hierarchy);
    }
}
