using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.LifeHistoryFeature.Create;

public class CreateLifeHistoryValidator : AbstractValidator<CreateLifeHistoryCommand>
{
    public CreateLifeHistoryValidator(AppDbContext db)
    {
        RuleFor(x => x.SuspectId).NotEmpty();

        RuleFor(x => x.SuspectId)
            .MustAsync(async (id, ct) => await db.Suspects.AnyAsync(s => s.Id == id, ct))
            .WithMessage("Suspect does not exist.");
    }
}
