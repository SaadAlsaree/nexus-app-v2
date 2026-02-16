using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Features.OperationFeature.GetById;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.OperationFeature.GetBySuspectId;

public sealed class GetOperationsBySuspectIdHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<List<OperationDto>>> Handle(GetOperationsBySuspectIdQuery query)
    {
        var operations = await _db.Operations
            .AsNoTracking()
            .Where(o => o.SuspectId == query.SuspectId && !o.IsDeleted)
            .OrderByDescending(o => o.Date)
            .ToListAsync();

        var dtos = _mapper.Map<List<OperationDto>>(operations);

        return Response<List<OperationDto>>.Success(dtos);
    }
}
