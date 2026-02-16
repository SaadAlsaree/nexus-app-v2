using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.OperationFeature.Create;

public sealed class CreateOperationHandler(AppDbContext _db)
{
    public async Task<(Response<Guid>, OperationCreatedEvent)> Handle(CreateOperationCommand command)
    {
        var operation = new Operation
        {
            Id = Guid.NewGuid(),
            SuspectId = command.SuspectId,
            OperationType = command.OperationType,
            Date = command.Date,
            Location = command.Location,
            RoleInOp = command.RoleInOp,
            Associates = command.Associates,
            CreatedAt = DateTime.UtcNow
        };

        _db.Operations.Add(operation);
        await _db.SaveChangesAsync();

        return (Response<Guid>.Success(operation.Id), new OperationCreatedEvent(operation.Id, operation.SuspectId));
    }
}
