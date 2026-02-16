using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.InterrogationSessionFeature.Create;

public sealed class CreateInterrogationSessionHandler(AppDbContext _db)
{
    public async Task<(Response<Guid>, InterrogationSessionCreatedEvent)> Handle(CreateInterrogationSessionCommand command)
    {
        var session = new InterrogationSession
        {
            Id = Guid.NewGuid(),
            SuspectId = command.SuspectId,
            CaseId = command.CaseId,
            SessionDate = command.SessionDate,
            InterrogatorName = command.InterrogatorName,
            Location = command.Location,
            SessionType = command.SessionType,
            Content = command.Content,
            SummaryContent = command.SummaryContent,
            Outcome = command.Outcome,
            InvestigatorNotes = command.InvestigatorNotes,
            IsRatifiedByJudge = command.IsRatifiedByJudge,
            CreatedAt = DateTime.UtcNow
        };

        _db.InterrogationSessions.Add(session);
        await _db.SaveChangesAsync();

        return (Response<Guid>.Success(session.Id), new InterrogationSessionCreatedEvent(session.Id, session.SuspectId));
    }
}
