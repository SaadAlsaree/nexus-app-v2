using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.SuspectFeature.Create;

public class CreateSuspectValidator : AbstractValidator<CreateSuspectCommand>
{
    public CreateSuspectValidator(AppDbContext db)
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.SecondName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ThirdName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.LegalArticle).MaximumLength(50);

        RuleFor(x => x)
            .MustAsync(async (cmd, ct) =>
            {
                var exists = await db.Suspects
                    .AnyAsync(s => s.FirstName == cmd.FirstName &&
                                   s.SecondName == cmd.SecondName &&
                                   s.ThirdName == cmd.ThirdName &&
                                   s.FivthName == cmd.FivthName, ct);
                return !exists;
            })
            .WithMessage("A suspect with this name combination already exists.");
    }
}
