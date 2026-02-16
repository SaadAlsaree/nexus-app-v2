using MapsterMapper;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.CaseFeature.UpdateCase;

public sealed class UpdateCaseHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<CaseUpdatedEvent>> Handle(UpdateCaseCommand command)
    {
        var @case = await _db.Cases.FindAsync(command.CaseId);
        if (@case == null)
            return Response<CaseUpdatedEvent>.Failure(Error.NotFound("Case.NotFound", "Case not found."));

        _mapper.Map(command, @case);
        @case.LastUpdateAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<CaseUpdatedEvent>.Success(new CaseUpdatedEvent(@case.Id));
    }
}
