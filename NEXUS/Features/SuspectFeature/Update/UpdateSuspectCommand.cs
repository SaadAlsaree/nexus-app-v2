using NEXUS.Data.Enums;

namespace NEXUS.Features.SuspectFeature.Update;

public record UpdateSuspectCommand(
    Guid SuspectId,
    string FirstName,
    string SecondName,
    string ThirdName,
    string? FourthName,
    string? FivthName,
    string? Kunya,
    string? CodeNum,
    string? MotherName,
    DateOnly? DateOfBirth,
    string? PlaceOfBirth,
    string? Tribe,
    MaritalStatus? MaritalStatus,
    int WivesCount,
    int ChildrenCount,
    string? LegalArticle,
    string? HealthStatus,
    string? PhotoUrl,
    SuspectStatus Status
);

public record SuspectUpdatedEvent(Guid SuspectId);
