using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.ContactFeature.CreateSuspectContact;

public sealed class CreateSuspectContactHandler(AppDbContext _db)
{
    public async Task<(Response<Guid>, SuspectContactCreatedEvent)> Handle(CreateSuspectContactCommand command)
    {
        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            SuspectId = command.SuspectId,
            Type = command.Type,
            Value = command.Value,
            OwnerName = command.OwnerName,
            CreatedAt = DateTime.UtcNow
        };

        _db.Contacts.Add(contact);
        await _db.SaveChangesAsync();

        return (Response<Guid>.Success(contact.Id), new SuspectContactCreatedEvent(
            contact.Id,
            contact.SuspectId,
            contact.Type,
            contact.Value));
    }
}
