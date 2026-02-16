using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.InterrogationSessionFeature.GetById;

public sealed class GetInterrogationSessionByIdHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<InterrogationSessionDto>> Handle(GetInterrogationSessionByIdQuery query)
    {
        var session = await _db.InterrogationSessions
            .AsNoTracking()
            .Include(s => s.Suspect)
            .Where(s => s.Id == query.Id && !s.IsDeleted)
            .FirstOrDefaultAsync();

        if (session == null)
            return Response<InterrogationSessionDto>.Failure(Error.NotFound("InterrogationSession.NotFound", "Interrogation session not found."));

        var dto = _mapper.Map<InterrogationSessionDto>(session);

        return Response<InterrogationSessionDto>.Success(dto);
    }
}
