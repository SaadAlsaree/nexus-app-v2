using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.OperationFeature.Create;

public class CreateOperationValidator : AbstractValidator<CreateOperationCommand>
{
    public CreateOperationValidator(AppDbContext db)
    {
        RuleFor(x => x.SuspectId).NotEmpty();
        RuleFor(x => x.OperationType).IsInEnum();
        RuleFor(x => x.Location).MaximumLength(200);
        RuleFor(x => x.RoleInOp).MaximumLength(100);

        RuleFor(x => x.SuspectId)
            .MustAsync(async (id, ct) => await db.Suspects.AnyAsync(s => s.Id == id, ct))
            .WithMessage("Suspect does not exist.");
    }
}
