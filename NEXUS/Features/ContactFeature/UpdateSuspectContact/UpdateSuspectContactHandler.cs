using MapsterMapper;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.ContactFeature.UpdateSuspectContact;

public sealed class UpdateSuspectContactHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<SuspectContactUpdatedEvent>> Handle(UpdateSuspectContactCommand command)
    {
        var contact = await _db.Contacts.FindAsync(command.ContactId);
        if (contact == null)
            return Response<SuspectContactUpdatedEvent>.Failure(Error.NotFound("Contact.NotFound", "Contact not found."));

        _mapper.Map(command, contact);
        contact.LastUpdateAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<SuspectContactUpdatedEvent>.Success(new SuspectContactUpdatedEvent(contact.Id));
    }
}
