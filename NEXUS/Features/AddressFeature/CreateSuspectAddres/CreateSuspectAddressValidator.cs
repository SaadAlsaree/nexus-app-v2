using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.AddressFeature.CreateSuspectAddres;

public class CreateSuspectAddressValidator : AbstractValidator<CreateSuspectAddressCommand>
{
    public CreateSuspectAddressValidator(AppDbContext db)
    {
        RuleFor(x => x.SuspectId).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.District).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Details).MaximumLength(500);

        RuleFor(x => x.SuspectId)
            .MustAsync(async (id, ct) => await db.Suspects.AnyAsync(s => s.Id == id, ct))
            .WithMessage("Suspect does not exist.");
    }
}
