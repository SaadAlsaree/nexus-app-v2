using FluentValidation;

namespace NEXUS.Features.SuspectFeature.Update;

public class UpdateSuspectValidator : AbstractValidator<UpdateSuspectCommand>
{
    public UpdateSuspectValidator()
    {
        RuleFor(x => x.SuspectId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.SecondName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ThirdName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.FivthName).MaximumLength(50);

        RuleFor(x => x.FourthName).MaximumLength(50);
        RuleFor(x => x.Kunya).MaximumLength(100);
        RuleFor(x => x.MotherName).MaximumLength(100);
        RuleFor(x => x.PlaceOfBirth).MaximumLength(200);
        RuleFor(x => x.Tribe).MaximumLength(100);
        RuleFor(x => x.LegalArticle).MaximumLength(100);
        RuleFor(x => x.HealthStatus).MaximumLength(500);
        RuleFor(x => x.PhotoUrl).MaximumLength(1000);
    }
}
