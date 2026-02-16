using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.TrainingCourseFeature.Create;

public sealed class CreateTrainingCourseHandler(AppDbContext _db)
{
    public async Task<(Response<Guid>, TrainingCourseCreatedEvent)> Handle(CreateTrainingCourseCommand command)
    {
        var course = new TrainingCourse
        {
            Id = Guid.NewGuid(),
            SuspectId = command.SuspectId,
            CourseType = command.CourseType,
            Location = command.Location,
            TrainerName = command.TrainerName,
            Classmates = command.Classmates,
            CreatedAt = DateTime.UtcNow
        };

        _db.TrainingCourses.Add(course);
        await _db.SaveChangesAsync();

        return (Response<Guid>.Success(course.Id), new TrainingCourseCreatedEvent(course.Id, course.SuspectId));
    }
}
