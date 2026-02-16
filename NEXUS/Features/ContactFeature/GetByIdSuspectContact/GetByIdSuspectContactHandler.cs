using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.ContactFeature.GetByIdSuspectContact;

public sealed class GetByIdSuspectContactHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<ContactDto>> Handle(GetByIdSuspectContactQuery query)
    {
        var contact = await _db.Contacts
            .AsNoTracking()
            .Where(c => c.Id == query.Id && !c.IsDeleted)
            .FirstOrDefaultAsync();

        if (contact == null)
            return Response<ContactDto>.Failure(Error.NotFound("Contact.NotFound", "Contact not found."));

        var dto = _mapper.Map<ContactDto>(contact);

        return Response<ContactDto>.Success(dto);
    }
}
