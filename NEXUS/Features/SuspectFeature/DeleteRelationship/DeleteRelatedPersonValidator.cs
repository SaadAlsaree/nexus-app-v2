using FluentValidation;

namespace NEXUS.Features.SuspectFeature.DeleteRelationship;

public class DeleteRelatedPersonValidator : AbstractValidator<DeleteRelatedPersonCommand>
{
    public DeleteRelatedPersonValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.SuspectId).NotEmpty();
    }
}
