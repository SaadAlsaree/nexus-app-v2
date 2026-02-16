using NEXUS.Data.Enums;

namespace NEXUS.Features.OperationFeature.Update;

public record UpdateOperationCommand(
    Guid OperationId,
    OperationType OperationType,
    DateOnly? Date,
    string? Location,
    string? RoleInOp,
    string? Associates
);

public record OperationUpdatedEvent(Guid OperationId);
