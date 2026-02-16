using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.ContactFeature.CreateSuspectContact;

public class CreateSuspectContactValidator : AbstractValidator<CreateSuspectContactCommand>
{
    public CreateSuspectContactValidator(AppDbContext db)
    {
        RuleFor(x => x.SuspectId).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Value).NotEmpty().MaximumLength(100);
        RuleFor(x => x.OwnerName).MaximumLength(100);

        RuleFor(x => x.SuspectId)
            .MustAsync(async (id, ct) => await db.Suspects.AnyAsync(s => s.Id == id, ct))
            .WithMessage("Suspect does not exist.");
    }
}
