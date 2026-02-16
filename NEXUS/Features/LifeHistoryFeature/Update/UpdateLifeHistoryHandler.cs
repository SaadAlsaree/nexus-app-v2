using MapsterMapper;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.LifeHistoryFeature.Update;

public sealed class UpdateLifeHistoryHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<LifeHistoryUpdatedEvent>> Handle(UpdateLifeHistoryCommand command)
    {
        var lifeHistory = await _db.LifeHistories.FindAsync(command.LifeHistoryId);
        if (lifeHistory == null)
            return Response<LifeHistoryUpdatedEvent>.Failure(Error.NotFound("LifeHistory.NotFound", "Life history record not found."));

        _mapper.Map(command, lifeHistory);
        lifeHistory.LastUpdateAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<LifeHistoryUpdatedEvent>.Success(new LifeHistoryUpdatedEvent(lifeHistory.Id));
    }
}
