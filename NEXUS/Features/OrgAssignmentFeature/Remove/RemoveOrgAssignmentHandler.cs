using NEXUS.Data;
using NEXUS.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace NEXUS.Features.OrgAssignmentFeature.Remove;

public sealed class RemoveOrgAssignmentHandler(AppDbContext _db)
{
    public async Task<Response<OrgAssignmentRemovedEvent>> Handle(RemoveOrgAssignmentCommand command)
    {
        var assignment = await _db.Assignments
            .FirstOrDefaultAsync(a => a.Id == command.Id && !a.IsDeleted);

        if (assignment == null)
            return Response<OrgAssignmentRemovedEvent>.Failure(Error.NotFound("OrgAssignment.NotFound", "Assignment not found."));

        assignment.IsDeleted = true;
        assignment.DeletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<OrgAssignmentRemovedEvent>.Success(new OrgAssignmentRemovedEvent(assignment.Id));
    }
}
