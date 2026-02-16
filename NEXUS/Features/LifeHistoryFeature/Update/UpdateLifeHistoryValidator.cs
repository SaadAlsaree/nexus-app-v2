using FluentValidation;

namespace NEXUS.Features.LifeHistoryFeature.Update;

public class UpdateLifeHistoryValidator : AbstractValidator<UpdateLifeHistoryCommand>
{
    public UpdateLifeHistoryValidator()
    {
        RuleFor(x => x.LifeHistoryId).NotEmpty();
    }
}
