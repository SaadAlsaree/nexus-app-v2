using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace NEXUS.Features.OrgUnitFeature.Create;

public sealed class CreateOrgUnitHandler(AppDbContext _db)
{
    public async Task<(Response<Guid>, OrgUnitCreatedEvent)> Handle(CreateOrgUnitCommand command)
    {
        string path = command.UnitName;

        if (command.ParentUnitId.HasValue)
        {
            var parent = await _db.OrgUnits
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == command.ParentUnitId.Value);

            if (parent == null)
                return (Response<Guid>.Failure(Error.NotFound("OrgUnit.ParentNotFound", "Parent unit not found.")), null!);

            path = $"{parent.Path} -> {command.UnitName}";
        }

        var orgUnit = new OrgUnit
        {
            Id = Guid.NewGuid(),
            UnitName = command.UnitName,
            Level = command.Level,
            ParentUnitId = command.ParentUnitId,
            Path = path,
            CreatedAt = DateTime.UtcNow
        };

        _db.OrgUnits.Add(orgUnit);
        await _db.SaveChangesAsync();

        return (Response<Guid>.Success(orgUnit.Id), new OrgUnitCreatedEvent(orgUnit.Id, orgUnit.UnitName, orgUnit.Path));
    }
}
