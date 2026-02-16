using MapsterMapper;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.TrainingCourseFeature.Update;

public sealed class UpdateTrainingCourseHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<TrainingCourseUpdatedEvent>> Handle(UpdateTrainingCourseCommand command)
    {
        var course = await _db.TrainingCourses.FindAsync(command.TrainingCourseId);
        if (course == null)
            return Response<TrainingCourseUpdatedEvent>.Failure(Error.NotFound("TrainingCourse.NotFound", "Training course not found."));

        _mapper.Map(command, course);
        course.LastUpdateAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<TrainingCourseUpdatedEvent>.Success(new TrainingCourseUpdatedEvent(course.Id));
    }
}
