using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.InterrogationSessionFeature.Create;

public class CreateInterrogationSessionValidator : AbstractValidator<CreateInterrogationSessionCommand>
{
    public CreateInterrogationSessionValidator(AppDbContext db)
    {
        RuleFor(x => x.SuspectId).NotEmpty();
        RuleFor(x => x.CaseId).NotEmpty();
        RuleFor(x => x.SessionDate).NotEmpty();
        RuleFor(x => x.InterrogatorName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SessionType).IsInEnum();
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.Outcome).IsInEnum();

        RuleFor(x => x.SuspectId)
            .MustAsync(async (id, ct) => await db.Suspects.AnyAsync(s => s.Id == id, ct))
            .WithMessage("Suspect does not exist.");

        RuleFor(x => x.CaseId)
            .MustAsync(async (id, ct) => await db.Cases.AnyAsync(c => c.Id == id, ct))
            .WithMessage("Case does not exist.");
    }
}
