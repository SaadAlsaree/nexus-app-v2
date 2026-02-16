using NEXUS.Data.Enums;
using NEXUS.Dtos;

namespace NEXUS.Events;

public record SuspectCreatedEvent(
    Guid Id,
    string FullName,
    string FirstName,
    string SecondName,
    string ThirdName,
    string? FourthName,
    string Tribe,
    string? CodeNum,
    string? Kunya,
    string? MotherName,
    DateOnly? DateOfBirth,
    string? PlaceOfBirth,
    SuspectStatus Status,
    string? CaseNumber,
    string? Governorate,
    List<HistoryEventDto> HistoryEvents
    );