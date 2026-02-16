using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Features.TrainingCourseFeature.GetById;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.TrainingCourseFeature.GetBySuspectId;

public sealed class GetTrainingCoursesBySuspectIdHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<List<TrainingCourseDto>>> Handle(GetTrainingCoursesBySuspectIdQuery query)
    {
        var courses = await _db.TrainingCourses
            .AsNoTracking()
            .Where(c => c.SuspectId == query.SuspectId && !c.IsDeleted)
            .ToListAsync();

        var dtos = _mapper.Map<List<TrainingCourseDto>>(courses);

        return Response<List<TrainingCourseDto>>.Success(dtos);
    }
}
