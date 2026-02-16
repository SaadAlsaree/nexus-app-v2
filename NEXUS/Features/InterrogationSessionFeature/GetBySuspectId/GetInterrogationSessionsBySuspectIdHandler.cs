using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Features.InterrogationSessionFeature.GetById;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.InterrogationSessionFeature.GetBySuspectId;

public sealed class GetInterrogationSessionsBySuspectIdHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<List<InterrogationSessionDto>>> Handle(GetInterrogationSessionsBySuspectIdQuery query)
    {
        var sessions = await _db.InterrogationSessions
            .AsNoTracking()
            .Where(s => s.SuspectId == query.SuspectId && !s.IsDeleted)
            .OrderByDescending(s => s.SessionDate)
            .ToListAsync();

        var dtos = _mapper.Map<List<InterrogationSessionDto>>(sessions);

        return Response<List<InterrogationSessionDto>>.Success(dtos);
    }
}
