using MapsterMapper;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.OperationFeature.Update;

public sealed class UpdateOperationHandler(AppDbContext _db, IMapper _mapper)
{
    public async Task<Response<OperationUpdatedEvent>> Handle(UpdateOperationCommand command)
    {
        var operation = await _db.Operations.FindAsync(command.OperationId);
        if (operation == null)
            return Response<OperationUpdatedEvent>.Failure(Error.NotFound("Operation.NotFound", "Operation not found."));

        _mapper.Map(command, operation);
        operation.LastUpdateAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<OperationUpdatedEvent>.Success(new OperationUpdatedEvent(operation.Id));
    }
}
