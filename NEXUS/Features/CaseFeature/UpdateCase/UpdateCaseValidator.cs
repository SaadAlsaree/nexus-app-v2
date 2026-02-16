using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.CaseFeature.UpdateCase;

public class UpdateCaseValidator : AbstractValidator<UpdateCaseCommand>
{
    public UpdateCaseValidator(AppDbContext db)
    {
        RuleFor(x => x.CaseId).NotEmpty();
        RuleFor(x => x.CaseFileNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.InvestigatingOfficer).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.Priority).IsInEnum();

        RuleFor(x => x)
            .MustAsync(async (cmd, ct) => !await db.Cases
                .AnyAsync(c => c.CaseFileNumber == cmd.CaseFileNumber && c.Id != cmd.CaseId, ct))
            .WithMessage("Another case with this file number already exists.");
    }
}
