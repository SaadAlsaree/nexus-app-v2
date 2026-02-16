using FluentValidation;

namespace NEXUS.Features.EvidenceAttachmentFeature.Update;

public class UpdateEvidenceValidator : AbstractValidator<UpdateEvidenceCommand>
{
    public UpdateEvidenceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.FileName)
            .MaximumLength(255)
            .When(x => x.FileName != null);
    }
}
