using FluentValidation;

namespace NEXUS.Features.AddressFeature.UpdateSuspectAddres;

public class UpdateSuspectAddressValidator : AbstractValidator<UpdateSuspectAddressCommand>
{
    public UpdateSuspectAddressValidator()
    {
        RuleFor(x => x.AddressId).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.District).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Details).MaximumLength(500);
    }
}
