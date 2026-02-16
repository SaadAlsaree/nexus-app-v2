namespace NEXUS.Features.OperationFeature.Delete;

public record SoftDeleteOperationCommand(Guid Id);

public record OperationDeletedEvent(Guid Id);
