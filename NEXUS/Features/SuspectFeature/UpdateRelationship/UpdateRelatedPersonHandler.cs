using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.SuspectFeature.UpdateRelationship;

public sealed class UpdateRelatedPersonHandler(AppDbContext db, IMapper _mapper)
{
    public async Task<Response<RelationshipUpdatedEvent>> Handle(UpdateRelatedPersonCommand command)
    {
        var person = await db.RelatedPeople
            .FirstOrDefaultAsync(x => x.Id == command.Id && x.SuspectId == command.SuspectId);

        if (person == null)
        {
            return Response<RelationshipUpdatedEvent>.Failure(Error.NotFound("Relationship.NotFound", "Relationship not found"));
        }

        _mapper.Map(command, person);
        person.UpdateFullName();

        await db.SaveChangesAsync();

        return Response<RelationshipUpdatedEvent>.Success(new RelationshipUpdatedEvent(command.SuspectId, person.Id, command.Relationship));
    }
}
