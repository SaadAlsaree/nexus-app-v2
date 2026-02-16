using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.CaseFeature.AddSupectToCase;

public class AddSuspectToCaseValidator : AbstractValidator<AddSuspectToCaseCommand>
{
    public AddSuspectToCaseValidator(AppDbContext db)
    {
        RuleFor(x => x.CaseId).NotEmpty();
        RuleFor(x => x.SuspectId).NotEmpty();
        RuleFor(x => x.AccusationType).IsInEnum();
        RuleFor(x => x.LegalStatus).IsInEnum();

        RuleFor(x => x.CaseId)
            .MustAsync(async (id, ct) => await db.Cases.AnyAsync(c => c.Id == id, ct))
            .WithMessage("Case does not exist.");

        RuleFor(x => x.SuspectId)
            .MustAsync(async (id, ct) => await db.Suspects.AnyAsync(s => s.Id == id, ct))
            .WithMessage("Suspect does not exist.");
    }
}
