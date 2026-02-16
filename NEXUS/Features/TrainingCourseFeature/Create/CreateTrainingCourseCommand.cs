using NEXUS.Data.Enums;

namespace NEXUS.Features.TrainingCourseFeature.Create;

public record CreateTrainingCourseCommand(
    Guid SuspectId,
    CourseType CourseType,
    string? Location,
    string? TrainerName,
    string? Classmates
);

public record TrainingCourseCreatedEvent(Guid TrainingCourseId, Guid SuspectId);
