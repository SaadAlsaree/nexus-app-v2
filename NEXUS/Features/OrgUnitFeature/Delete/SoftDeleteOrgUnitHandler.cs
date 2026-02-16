using NEXUS.Data;
using NEXUS.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace NEXUS.Features.OrgUnitFeature.Delete;

public sealed class SoftDeleteOrgUnitHandler(AppDbContext _db)
{
    public async Task<Response<OrgUnitDeletedEvent>> Handle(SoftDeleteOrgUnitCommand command)
    {
        var unit = await _db.OrgUnits
            .Include(u => u.SubUnits)
            .FirstOrDefaultAsync(u => u.Id == command.Id);

        if (unit == null)
            return Response<OrgUnitDeletedEvent>.Failure(Error.NotFound("OrgUnit.NotFound", "Unit not found."));

        // Check if it has active sub-units
        if (unit.SubUnits.Any(s => !s.IsDeleted))
        {
            return Response<OrgUnitDeletedEvent>.Failure(Error.Conflict("OrgUnit.HasChildren", "Cannot delete a unit that has active sub-units."));
        }

        unit.IsDeleted = true;
        unit.DeletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<OrgUnitDeletedEvent>.Success(new OrgUnitDeletedEvent(unit.Id));
    }
}
