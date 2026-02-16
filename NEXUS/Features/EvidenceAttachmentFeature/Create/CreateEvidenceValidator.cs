using FluentValidation;

namespace NEXUS.Features.EvidenceAttachmentFeature.Create;

public class CreateEvidenceValidator : AbstractValidator<CreateEvidenceCommand>
{
    public CreateEvidenceValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required.")
            .Must(f => f.Length > 0)
            .WithMessage("File cannot be empty.");

        RuleFor(x => x)
            .Must(x => x.CaseId.HasValue || x.SuspectId.HasValue)
            .WithMessage("Evidence must be linked to either a Case or a Suspect.");
    }
}
