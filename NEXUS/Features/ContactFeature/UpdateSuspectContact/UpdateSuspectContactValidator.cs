using FluentValidation;

namespace NEXUS.Features.ContactFeature.UpdateSuspectContact;

public class UpdateSuspectContactValidator : AbstractValidator<UpdateSuspectContactCommand>
{
    public UpdateSuspectContactValidator()
    {
        RuleFor(x => x.ContactId).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Value).NotEmpty().MaximumLength(100);
        RuleFor(x => x.OwnerName).MaximumLength(100);
    }
}
