using NEXUS.Data.Enums;

namespace NEXUS.Features.TrainingCourseFeature.Update;

public record UpdateTrainingCourseCommand(
    Guid TrainingCourseId,
    CourseType CourseType,
    string? Location,
    string? TrainerName,
    string? Classmates
);

public record TrainingCourseUpdatedEvent(Guid TrainingCourseId);
