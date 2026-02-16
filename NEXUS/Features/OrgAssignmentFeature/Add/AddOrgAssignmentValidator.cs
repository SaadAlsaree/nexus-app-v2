using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.OrgAssignmentFeature.Add;

public class AddOrgAssignmentValidator : AbstractValidator<AddOrgAssignmentCommand>
{
    public AddOrgAssignmentValidator(AppDbContext db)
    {
        RuleFor(x => x.SuspectId).NotEmpty();
        RuleFor(x => x.OrgUnitId).NotEmpty();
        RuleFor(x => x.RoleTitle).IsInEnum();
        RuleFor(x => x.StartDate).NotEmpty();

        RuleFor(x => x)
            .MustAsync(async (cmd, ct) => !await db.Assignments
                .AnyAsync(a => a.SuspectId == cmd.SuspectId
                               && a.OrgUnitId == cmd.OrgUnitId
                               && a.RoleTitle == cmd.RoleTitle
                               && a.EndDate == null
                               && !a.IsDeleted, ct))
            .WithMessage("This suspect is already assigned to this unit with the same role and no end date.");
    }
}
