using FluentValidation;

namespace NEXUS.Features.TrainingCourseFeature.Update;

public class UpdateTrainingCourseValidator : AbstractValidator<UpdateTrainingCourseCommand>
{
    public UpdateTrainingCourseValidator()
    {
        RuleFor(x => x.TrainingCourseId).NotEmpty();
        RuleFor(x => x.CourseType).IsInEnum();
        RuleFor(x => x.Location).MaximumLength(200);
        RuleFor(x => x.TrainerName).MaximumLength(100);
    }
}
