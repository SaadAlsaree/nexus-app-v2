using NEXUS.Data.Enums;

namespace NEXUS.Features.SuspectFeature.SuspectList;

public record SuspectListDto(
    Guid Id,
    string FullName,
    DateOnly? DateOfBirth,
    SuspectStatus CurrentStatus,
    string? CurrentStatusName,
    string? Kunya,
    string? MotherName,
    string? PlaceOfBirth,
    string? Tribe,
    MaritalStatus? MaritalStatus,
    string? MaritalStatusName,
    int WivesCount,
    int ChildrenCount,
    string? LegalArticle,
    string? PhotoUrl,
    DateTime CreatedAt
);
