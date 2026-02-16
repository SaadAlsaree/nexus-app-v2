using NEXUS.Data.Enums;

namespace NEXUS.Features.SuspectFeature.AddRelationship;

public record AddRelatedPersonCommand(
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

public record RelationshipAddedEvent(Guid SuspectId, Guid RelatedPersonId, RelationshipType Relationship);
