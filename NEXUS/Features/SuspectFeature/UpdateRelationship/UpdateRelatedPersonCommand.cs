using NEXUS.Data.Enums;

namespace NEXUS.Features.SuspectFeature.UpdateRelationship;

public record UpdateRelatedPersonCommand(
    Guid Id,
    Guid SuspectId,
    string FirstName,
    string? SecondName,
    string? ThirdName,
    string? FourthName,
    string? FivthName,
    string Tribe,
    RelationshipType Relationship,
    string? Notes
);

public record RelationshipUpdatedEvent(Guid SuspectId, Guid RelatedPersonId, RelationshipType Relationship);
