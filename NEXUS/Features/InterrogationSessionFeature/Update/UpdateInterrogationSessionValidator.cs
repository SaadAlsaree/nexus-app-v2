using FluentValidation;
using NEXUS.Data;

namespace NEXUS.Features.InterrogationSessionFeature.Update;

public class UpdateInterrogationSessionValidator : AbstractValidator<UpdateInterrogationSessionCommand>
{
    public UpdateInterrogationSessionValidator()
    {
        RuleFor(x => x.InterrogationSessionId).NotEmpty();
        RuleFor(x => x.SessionDate).NotEmpty();
        RuleFor(x => x.InterrogatorName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SessionType).IsInEnum();
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.Outcome).IsInEnum();
    }
}
