namespace NEXUS.Features.TrainingCourseFeature.Delete;

public record SoftDeleteTrainingCourseCommand(Guid TrainingCourseId);

public record TrainingCourseDeletedEvent(Guid TrainingCourseId);
