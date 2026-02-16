using FluentValidation;

namespace NEXUS.Features.SuspectFeature.AddRelationship;

public class AddRelatedPersonValidator : AbstractValidator<AddRelatedPersonCommand>
{
    public AddRelatedPersonValidator()
    {
        RuleFor(x => x.SuspectId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.SecondName).MaximumLength(50);
        RuleFor(x => x.ThirdName).MaximumLength(50);
        RuleFor(x => x.FourthName).MaximumLength(50);
        RuleFor(x => x.FivthName).MaximumLength(50);
        RuleFor(x => x.Tribe).MaximumLength(100);
        RuleFor(x => x.Relationship).IsInEnum();
    }
}
