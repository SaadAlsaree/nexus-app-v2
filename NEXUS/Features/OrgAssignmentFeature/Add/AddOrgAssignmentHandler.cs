using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace NEXUS.Features.OrgAssignmentFeature.Add;

public sealed class AddOrgAssignmentHandler(AppDbContext _db)
{
    public async Task<(Response<Guid>, OrgAssignmentAddedEvent)> Handle(AddOrgAssignmentCommand command)
    {
        // Check if suspect exists
        var suspectExists = await _db.Suspects.AnyAsync(s => s.Id == command.SuspectId && !s.IsDeleted);
        if (!suspectExists)
            return (Response<Guid>.Failure(Error.NotFound("Suspect.NotFound", "Suspect not found.")), null!);

        // Check if org unit exists
        var orgUnitExists = await _db.OrgUnits.AnyAsync(u => u.Id == command.OrgUnitId && !u.IsDeleted);
        if (!orgUnitExists)
            return (Response<Guid>.Failure(Error.NotFound("OrgUnit.NotFound", "Organizational unit not found.")), null!);

        // Check if direct commander exists if provided
        if (command.DirectCommanderId.HasValue)
        {
            var commanderExists = await _db.Suspects.AnyAsync(s => s.Id == command.DirectCommanderId.Value && !s.IsDeleted);
            if (!commanderExists)
                return (Response<Guid>.Failure(Error.NotFound("Commander.NotFound", "Direct commander not found.")), null!);
        }

        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            SuspectId = command.SuspectId,
            OrgUnitId = command.OrgUnitId,
            DirectCommanderId = command.DirectCommanderId,
            RoleTitle = command.RoleTitle,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            CreatedAt = DateTime.UtcNow
        };

        _db.Assignments.Add(assignment);
        await _db.SaveChangesAsync();

        return (Response<Guid>.Success(assignment.Id), new OrgAssignmentAddedEvent(assignment.Id, assignment.SuspectId, assignment.OrgUnitId));
    }
}
