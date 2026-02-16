using Mapster;
using NEXUS.Data.Entities;
using NEXUS.Extensions;
using NEXUS.Features.TrainingCourseFeature.Create;
using NEXUS.Features.TrainingCourseFeature.GetById;
using NEXUS.Features.TrainingCourseFeature.Update;

namespace NEXUS.Mappings;

public class TrainingCourseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<TrainingCourse, TrainingCourseDto>()
            .Map(dest => dest.CourseTypeName, src => src.CourseType.GetDisplayName());

        config.NewConfig<CreateTrainingCourseCommand, TrainingCourse>()
            .Map(dest => dest.Id, src => Guid.NewGuid())
            .Map(dest => dest.CreatedAt, src => DateTime.UtcNow);

        config.NewConfig<UpdateTrainingCourseCommand, TrainingCourse>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt);
    }
}
