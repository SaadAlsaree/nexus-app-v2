using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.SuspectFeature.AddRelationship;

public sealed class AddRelatedPersonHandler(AppDbContext db)
{
    public async Task<Response<RelationshipAddedEvent>> Handle(AddRelatedPersonCommand command)
    {
        var person = new RelatedPerson
        {
            Id = Guid.NewGuid(),
            SuspectId = command.SuspectId,
            FirstName = command.FirstName,
            SecondName = command.SecondName ?? "",
            ThirdName = command.ThirdName ?? "",
            FourthName = command.FourthName,
            FivthName = command.FivthName,
            Tribe = command.Tribe,
            Relationship = command.Relationship,
            Notes = command.Notes,
            CreatedAt = DateTime.UtcNow
        };

        person.UpdateFullName();

        db.RelatedPeople.Add(person);
        await db.SaveChangesAsync();

        return Response<RelationshipAddedEvent>.Success(new RelationshipAddedEvent(command.SuspectId, person.Id, command.Relationship));
    }
}
