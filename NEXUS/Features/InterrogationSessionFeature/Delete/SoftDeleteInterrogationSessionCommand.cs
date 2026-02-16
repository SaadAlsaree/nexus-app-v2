namespace NEXUS.Features.InterrogationSessionFeature.Delete;

public record SoftDeleteInterrogationSessionCommand(Guid Id);

public record InterrogationSessionDeletedEvent(Guid Id);
