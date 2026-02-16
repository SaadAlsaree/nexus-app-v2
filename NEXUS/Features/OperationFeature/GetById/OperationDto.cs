using NEXUS.Data.Enums;

namespace NEXUS.Features.OperationFeature.GetById;

public record OperationDto(
    Guid Id,
    Guid SuspectId,
    OperationType OperationType,
    string OperationTypeName,
    DateOnly? Date,
    string? Location,
    string? RoleInOp,
    string? Associates,
    DateTime CreatedAt
);
