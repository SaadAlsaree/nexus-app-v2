using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.TrainingCourseFeature.GetById;

public sealed class GetTrainingCourseByIdHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<TrainingCourseDto>> Handle(GetTrainingCourseByIdQuery query)
    {
        var course = await _db.TrainingCourses
            .AsNoTracking()
            .Where(c => c.Id == query.Id && !c.IsDeleted)
            .FirstOrDefaultAsync();

        if (course == null)
            return Response<TrainingCourseDto>.Failure(Error.NotFound("TrainingCourse.NotFound", "Training course not found."));

        var dto = _mapper.Map<TrainingCourseDto>(course);

        return Response<TrainingCourseDto>.Success(dto);
    }
}
