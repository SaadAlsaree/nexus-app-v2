using NEXUS.Data.Enums;

namespace NEXUS.Features.OperationFeature.Create;

public record CreateOperationCommand(
    Guid SuspectId,
    OperationType OperationType,
    DateOnly? Date,
    string? Location,
    string? RoleInOp,
    string? Associates
);

public record OperationCreatedEvent(Guid OperationId, Guid SuspectId);
