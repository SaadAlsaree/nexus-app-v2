using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.OrgUnitFeature.Create;

public class CreateOrgUnitValidator : AbstractValidator<CreateOrgUnitCommand>
{
    public CreateOrgUnitValidator(AppDbContext db)
    {
        RuleFor(x => x.UnitName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Level).IsInEnum();

        RuleFor(x => x)
            .MustAsync(async (cmd, ct) => !await db.OrgUnits
                .AnyAsync(u => u.UnitName == cmd.UnitName && u.ParentUnitId == cmd.ParentUnitId && !u.IsDeleted, ct))
            .WithMessage("A unit with this name already exists under the same parent.");
    }
}
