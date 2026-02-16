using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.OperationFeature.Delete;

public sealed class SoftDeleteOperationHandler(AppDbContext _db)
{
    public async Task<Response<OperationDeletedEvent>> Handle(SoftDeleteOperationCommand command)
    {
        var operation = await _db.Operations.FindAsync(command.Id);
        if (operation == null)
            return Response<OperationDeletedEvent>.Failure(Error.NotFound("Operation.NotFound", "Operation not found."));

        operation.IsDeleted = true;
        operation.DeletedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Response<OperationDeletedEvent>.Success(new OperationDeletedEvent(operation.Id));
    }
}
