using FluentValidation;

namespace NEXUS.Features.SuspectFeature.UpdateRelationship;

public class UpdateRelatedPersonValidator : AbstractValidator<UpdateRelatedPersonCommand>
{
    public UpdateRelatedPersonValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.SuspectId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.SecondName).MaximumLength(50);
        RuleFor(x => x.ThirdName).MaximumLength(50);
        RuleFor(x => x.FourthName).MaximumLength(50);
        RuleFor(x => x.FivthName).MaximumLength(50);
        RuleFor(x => x.Tribe).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Relationship).IsInEnum();
    }
}
