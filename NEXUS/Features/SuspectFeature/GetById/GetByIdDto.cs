using NEXUS.Data.Enums;

namespace NEXUS.Features.SuspectFeature.GetById;

public record GetByIdDto(
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
    DateTime CreatedAt,
    List<AddressDto> Addresses,
    List<ContactDto> Contacts,
    List<RelatedPersonDto> RelativesAndAssociates,
    List<LifeHistoryDto> LifeHistories,
    List<BayahDetailDto> BayahDetails,
    List<TrainingCourseDto> TrainingCourses,
    List<OperationDto> Operations,
    List<AssignmentDto> OrganizationalAssignments,
    List<CaseInvolvementDto> CaseInvolvements
);

public record AddressDto(
    Guid Id,
    AddressType Type,
    string TypeName,
    string City,
    string District,
    string? Details,
    string? GPSCoordinates
);

public record ContactDto(
    Guid Id,
    ContactType Type,
    string TypeName,
    string Value,
    string? OwnerName
);

public record RelatedPersonDto(
    Guid Id,
    string FullName,
    string FirstName,
    string SecondName,
    string ThirdName,
    string? FourthName,
    string? FivthName,
    string? Tribe,
    RelationshipType Relationship,
    string RelationshipName,
    string? SpouseName,
    string? CurrentStatus,
    string? Notes
);

public record LifeHistoryDto(
    Guid Id,
    string? EducationLevel,
    string? SchoolsAttended,
    string? CivilianJobs,
    string? RadicalizationStory
);

public record BayahDetailDto(
    Guid Id,
    DateTime? Date,
    string? Location,
    string? RecruiterName,
    string? TextOfPledge
);

public record TrainingCourseDto(
    Guid Id,
    CourseType CourseType,
    string CourseTypeName,
    string? Location,
    string? TrainerName,
    string? Classmates
);

public record OperationDto(
    Guid Id,
    OperationType OperationType,
    string OperationTypeName,
    DateTime? Date,
    string? Location,
    string? RoleInOp,
    string? Associates
);

public record AssignmentDto(
    Guid Id,
    Guid OrgUnitId,
    string OrgUnitName,
    Guid? DirectCommanderId,
    string? DirectCommanderName,
    OrgRole RoleTitle,
    string RoleTitleName,
    DateTime StartDate,
    DateTime? EndDate
);

public record CaseInvolvementDto(
    Guid Id,
    Guid CaseId,
    string CaseFileNumber,
    string CaseTitle,
    AccusationType AccusationType,
    string AccusationTypeName,
    LegalStatusInCase LegalStatus,
    string LegalStatusName,
    DateTime? ConfessionDate,
    string? Notes
);
