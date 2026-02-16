using NEXUS.Data.Enums;

namespace NEXUS.Features.InterrogationSessionFeature.GetById;

public record InterrogationSessionDto(
    Guid Id,
    Guid SuspectId,
    SuspectDto Suspect,
    Guid CaseId,
    DateOnly SessionDate,
    string InterrogatorName,
    string Location,
    InterrogationType SessionType,
    string SessionTypeName,
    string Content,
    string SummaryContent,
    InterrogationOutcome Outcome,
    string OutcomeName,
    string? InvestigatorNotes,
    bool IsRatifiedByJudge,
    DateTime CreatedAt
);


public record SuspectDto(
    Guid Id,
    string FirstName,
    string SecondName,
    string ThirdName,
    string? FourthName,
    string? FivthName,
    string FullName,
    string? Kunya,
    string? MotherName,
    DateOnly? DateOfBirth,
    string? PlaceOfBirth,
    string? Tribe,
    MaritalStatus? MaritalStatus,
    string? MaritalStatusName,
    int WivesCount,
    int ChildrenCount,
    string? LegalArticle,
    string? HealthStatus,
    string? PhotoUrl,
    SuspectStatus CurrentStatus,
    string? CurrentStatusName,
    DateTime CreatedAt
);
