using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.LifeHistoryFeature.Create;

public sealed class CreateLifeHistoryHandler(AppDbContext _db)
{
    public async Task<(Response<Guid>, LifeHistoryCreatedEvent)> Handle(CreateLifeHistoryCommand command)
    {
        var lifeHistory = new LifeHistory
        {
            Id = Guid.NewGuid(),
            SuspectId = command.SuspectId,
            EducationLevel = command.EducationLevel,
            SchoolsAttended = command.SchoolsAttended,
            CivilianJobs = command.CivilianJobs,
            RadicalizationStory = command.RadicalizationStory,
            CreatedAt = DateTime.UtcNow
        };

        _db.LifeHistories.Add(lifeHistory);
        await _db.SaveChangesAsync();

        return (Response<Guid>.Success(lifeHistory.Id), new LifeHistoryCreatedEvent(lifeHistory.Id, lifeHistory.SuspectId));
    }
}
