using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.OperationFeature.GetById;

public sealed class GetOperationByIdHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<OperationDto>> Handle(GetOperationByIdQuery query)
    {
        var operation = await _db.Operations
            .AsNoTracking()
            .Where(o => o.Id == query.Id && !o.IsDeleted)
            .FirstOrDefaultAsync();

        if (operation == null)
            return Response<OperationDto>.Failure(Error.NotFound("Operation.NotFound", "Operation not found."));

        var dto = _mapper.Map<OperationDto>(operation);

        return Response<OperationDto>.Success(dto);
    }
}
