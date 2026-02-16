using MapsterMapper;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.InterrogationSessionFeature.Update;

public sealed class UpdateInterrogationSessionHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<InterrogationSessionUpdatedEvent>> Handle(UpdateInterrogationSessionCommand command)
    {
        var session = await _db.InterrogationSessions.FindAsync(command.InterrogationSessionId);
        if (session == null)
            return Response<InterrogationSessionUpdatedEvent>.Failure(Error.NotFound("InterrogationSession.NotFound", "Interrogation session not found."));

        _mapper.Map(command, session);
        session.LastUpdateAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<InterrogationSessionUpdatedEvent>.Success(new InterrogationSessionUpdatedEvent(session.Id));
    }
}
