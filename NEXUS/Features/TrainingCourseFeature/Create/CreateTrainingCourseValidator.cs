using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;

namespace NEXUS.Features.TrainingCourseFeature.Create;

public class CreateTrainingCourseValidator : AbstractValidator<CreateTrainingCourseCommand>
{
    public CreateTrainingCourseValidator(AppDbContext db)
    {
        RuleFor(x => x.SuspectId).NotEmpty();
        RuleFor(x => x.CourseType).IsInEnum();
        RuleFor(x => x.Location).MaximumLength(200);
        RuleFor(x => x.TrainerName).MaximumLength(100);

        RuleFor(x => x.SuspectId)
            .MustAsync(async (id, ct) => await db.Suspects.AnyAsync(s => s.Id == id, ct))
            .WithMessage("Suspect does not exist.");
    }
}
