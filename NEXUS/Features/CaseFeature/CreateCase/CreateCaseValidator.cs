using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.CaseFeature.CreateCase;

public class CreateCaseValidator : AbstractValidator<CreateCaseCommand>
{
    public CreateCaseValidator(AppDbContext db)
    {
        RuleFor(x => x.CaseFileNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.InvestigatingOfficer).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.Priority).IsInEnum();

        RuleFor(x => x.CaseFileNumber)
            .MustAsync(async (number, ct) => !await db.Cases.AnyAsync(c => c.CaseFileNumber == number, ct))
            .WithMessage("Case file number already exists.");
    }
}
