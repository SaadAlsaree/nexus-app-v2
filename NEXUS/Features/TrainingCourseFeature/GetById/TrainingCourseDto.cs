using NEXUS.Data.Enums;

namespace NEXUS.Features.TrainingCourseFeature.GetById;

public record TrainingCourseDto(
    Guid Id,
    Guid SuspectId,
    CourseType CourseType,
    string CourseTypeName,
    string? Location,
    string? TrainerName,
    string? Classmates,
    DateTime CreatedAt
);
