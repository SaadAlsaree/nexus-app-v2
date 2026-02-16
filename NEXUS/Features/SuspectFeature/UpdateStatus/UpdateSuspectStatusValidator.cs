using FluentValidation;

namespace NEXUS.Features.SuspectFeature.UpdateStatus;

public class UpdateSuspectStatusValidator : AbstractValidator<UpdateSuspectStatusCommand>
{
    public UpdateSuspectStatusValidator()
    {
        RuleFor(x => x.SuspectId).NotEmpty();
        RuleFor(x => x.NewStatus).IsInEnum();
    }
}
