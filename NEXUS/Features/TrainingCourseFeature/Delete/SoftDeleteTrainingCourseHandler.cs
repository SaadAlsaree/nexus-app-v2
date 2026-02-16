using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.TrainingCourseFeature.Delete;

public sealed class SoftDeleteTrainingCourseHandler(AppDbContext _db)
{
    public async Task<Response<TrainingCourseDeletedEvent>> Handle(SoftDeleteTrainingCourseCommand command)
    {
        var course = await _db.TrainingCourses.FindAsync(command.TrainingCourseId);
        if (course == null)
            return Response<TrainingCourseDeletedEvent>.Failure(Error.NotFound("TrainingCourse.NotFound", "Training course not found."));

        course.IsDeleted = true;
        course.DeletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<TrainingCourseDeletedEvent>.Success(new TrainingCourseDeletedEvent(course.Id));
    }
}
