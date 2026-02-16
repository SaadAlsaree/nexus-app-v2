using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.SuspectFeature.DeleteRelationship;

public sealed class DeleteRelatedPersonHandler(AppDbContext db)
{
    public async Task<Response<RelationshipDeletedEvent>> Handle(DeleteRelatedPersonCommand command)
    {
        var person = await db.RelatedPeople
            .FirstOrDefaultAsync(x => x.Id == command.Id && x.SuspectId == command.SuspectId);

        if (person == null)
        {
            return Response<RelationshipDeletedEvent>.Failure(Error.NotFound("Relationship.NotFound", "Relationship not found"));
        }

        db.RelatedPeople.Remove(person);
        await db.SaveChangesAsync();

        return Response<RelationshipDeletedEvent>.Success(new RelationshipDeletedEvent(command.SuspectId, person.Id));
    }
}
