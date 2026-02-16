using NEXUS.Data.Enums;

namespace NEXUS.Features.SuspectFeature.Create;

public record CreateSuspectCommand(
    string FirstName,
    string SecondName,
    string ThirdName,
    string? FourthName,
    string? FivthName,
    string? CodeNum,
    string? Kunya,
    string? MotherName,
    DateOnly? DateOfBirth,
    string? PlaceOfBirth,
    string? Tribe,
    SuspectStatus Status,
    string? LegalArticle
);

