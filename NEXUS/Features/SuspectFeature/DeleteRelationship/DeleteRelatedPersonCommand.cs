namespace NEXUS.Features.SuspectFeature.DeleteRelationship;

public record DeleteRelatedPersonCommand(Guid Id, Guid SuspectId);

public record RelationshipDeletedEvent(Guid SuspectId, Guid RelatedPersonId);
