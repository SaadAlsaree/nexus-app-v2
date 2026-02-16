using MapsterMapper;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.SuspectFeature.Update;

public sealed class UpdateSuspectHandler
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public UpdateSuspectHandler(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    public async Task<Response<SuspectUpdatedEvent>> Handle(UpdateSuspectCommand command)
    {
        var suspect = await _db.Suspects.FindAsync(command.SuspectId);
        if (suspect == null)
            return Response<SuspectUpdatedEvent>.Failure(Error.NotFound("Suspect.NotFound", "Suspect not found"));

        _mapper.Map(command, suspect);

        await _db.SaveChangesAsync();

        return Response<SuspectUpdatedEvent>.Success(new SuspectUpdatedEvent(suspect.Id));
    }
}
