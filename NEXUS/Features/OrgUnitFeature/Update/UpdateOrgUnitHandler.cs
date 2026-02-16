using NEXUS.Data;
using NEXUS.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using MapsterMapper;

namespace NEXUS.Features.OrgUnitFeature.Update;

public sealed class UpdateOrgUnitHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<OrgUnitUpdatedEvent>> Handle(UpdateOrgUnitCommand command)
    {
        var unit = await _db.OrgUnits.FindAsync(command.Id);
        if (unit == null)
            return Response<OrgUnitUpdatedEvent>.Failure(Error.NotFound("OrgUnit.NotFound", "Unit not found."));

        // Detect if parent or name changed to update path
        bool pathChanged = unit.UnitName != command.UnitName || unit.ParentUnitId != command.ParentUnitId;

_mapper.Map(command, unit);
        unit.LastUpdateAt = DateTime.UtcNow;

        if (pathChanged)
        {
            string oldPath = unit.Path;
            string newPath = command.UnitName;

            if (command.ParentUnitId.HasValue)
            {
                var parent = await _db.OrgUnits.AsNoTracking().FirstOrDefaultAsync(u => u.Id == command.ParentUnitId);
                if (parent != null) newPath = $"{parent.Path} -> {command.UnitName}";
            }

            unit.Path = newPath;

            // Re-calculate paths for children recursively (simplified here via direct DB update if path is key)
            // Note: In a complex tree, we'd need a recursive update for all descendants.
            var children = await _db.OrgUnits.Where(u => u.Path.StartsWith(oldPath + " -> ")).ToListAsync();
            foreach (var child in children)
            {
                child.Path = child.Path.Replace(oldPath, newPath);
            }
        }

        await _db.SaveChangesAsync();

        return Response<OrgUnitUpdatedEvent>.Success(new OrgUnitUpdatedEvent(unit.Id));
    }
}
