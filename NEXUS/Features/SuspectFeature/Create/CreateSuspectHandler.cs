using MapsterMapper;
using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Events;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.SuspectFeature.Create;

public sealed class CreateSuspectHandler
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public CreateSuspectHandler(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    public async Task<(Response<Guid>, SuspectCreatedEvent)> Handle(CreateSuspectCommand command)
    {
        var suspect = _mapper.Map<Suspect>(command);

        _db.Suspects.Add(suspect);
        await _db.SaveChangesAsync();

        var domainEvent = _mapper.Map<SuspectCreatedEvent>(suspect);

        return (Response<Guid>.Success(suspect.Id), domainEvent);
    }
}
