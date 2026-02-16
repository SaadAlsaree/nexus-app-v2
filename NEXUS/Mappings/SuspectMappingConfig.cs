using Mapster;
using NEXUS.Data.Entities;
using NEXUS.Events;
using NEXUS.Extensions;
using NEXUS.Features.SuspectFeature.Create;
using NEXUS.Features.SuspectFeature.GetById;
using NEXUS.Features.SuspectFeature.SuspectList;
using NEXUS.Features.SuspectFeature.Update;

namespace NEXUS.Mappings;

public class SuspectMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Global DateOnly mappings
        config.NewConfig<DateOnly, DateTime>().MapWith(src => src.ToDateTime(TimeOnly.MinValue));
        config.NewConfig<DateTime, DateOnly>().MapWith(src => DateOnly.FromDateTime(src));
        config.NewConfig<DateOnly?, DateTime?>().MapWith(src => src.HasValue ? src.Value.ToDateTime(TimeOnly.MinValue) : null);
        config.NewConfig<DateTime?, DateOnly?>().MapWith(src => src.HasValue ? DateOnly.FromDateTime(src.Value) : null);

        #region GetById
        config.NewConfig<Suspect, GetByIdDto>()
            .Map(dest => dest.MaritalStatusName, src => src.MaritalStatus != null ? src.MaritalStatus.Value.GetDisplayName() : null)
            .Map(dest => dest.CurrentStatusName, src => src.CurrentStatus.GetDisplayName())
            .Map(dest => dest.PhotoUrl, src =>
                !string.IsNullOrEmpty(src.PhotoUrl) && MapContext.Current != null && MapContext.Current.Parameters.ContainsKey("BaseUrl")
                ? $"{MapContext.Current.Parameters["BaseUrl"]}/api/storage/{src.PhotoUrl}"
                : src.PhotoUrl)
            ;

        // إعداد AssignmentDto (لجلب اسم الوحدة واسم القائد)
        config.NewConfig<Assignment, AssignmentDto>()
            .Map(dest => dest.OrgUnitName, src => src.OrgUnit.UnitName)
            .Map(dest => dest.DirectCommanderName, src => src.DirectCommander != null ? src.DirectCommander.FullName : null)
            .Map(dest => dest.RoleTitleName, src => src.RoleTitle.GetDisplayName());

        // إعداد CaseInvolvementDto (لجلب بيانات القضية)
        config.NewConfig<CaseSuspect, CaseInvolvementDto>()
            .Map(dest => dest.CaseFileNumber, src => src.Case.CaseFileNumber)
            .Map(dest => dest.CaseTitle, src => src.Case.Title)
            .Map(dest => dest.AccusationTypeName, src => src.AccusationType.GetDisplayName())
            .Map(dest => dest.LegalStatusName, src => src.LegalStatus.GetDisplayName());

        // إعدادات بسيطة لباقي الـ DTOs لجلب DisplayName للـ Enums
        config.NewConfig<Address, AddressDto>()
            .Map(dest => dest.TypeName, src => src.Type.GetDisplayName());

        config.NewConfig<Contact, ContactDto>()
            .Map(dest => dest.TypeName, src => src.Type.GetDisplayName());

        config.NewConfig<RelatedPerson, RelatedPersonDto>()
            .Map(dest => dest.RelationshipName, src => src.Relationship.GetDisplayName())
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.SecondName, src => src.SecondName)
            .Map(dest => dest.ThirdName, src => src.ThirdName)
            .Map(dest => dest.FourthName, src => src.FourthName)
            .Map(dest => dest.FivthName, src => src.FivthName)
            .Map(dest => dest.Tribe, src => src.Tribe);

        config.NewConfig<TrainingCourse, TrainingCourseDto>()
            .Map(dest => dest.CourseTypeName, src => src.CourseType.GetDisplayName());

        config.NewConfig<Operation, OperationDto>()
            .Map(dest => dest.OperationTypeName, src => src.OperationType.GetDisplayName());
        #endregion

        #region Suspect List
        config.NewConfig<Suspect, SuspectListDto>()
           .Map(dest => dest.MaritalStatusName, src => src.MaritalStatus != null ? src.MaritalStatus.Value.GetDisplayName() : null)
           .Map(dest => dest.CurrentStatusName, src => src.CurrentStatus.GetDisplayName())
           .Map(dest => dest.PhotoUrl, src =>
               !string.IsNullOrEmpty(src.PhotoUrl) && MapContext.Current != null && MapContext.Current.Parameters.ContainsKey("BaseUrl")
               ? $"{MapContext.Current.Parameters["BaseUrl"]}/api/storage/{src.PhotoUrl}"
               : src.PhotoUrl);
        #endregion

        #region Create
        config.NewConfig<CreateSuspectCommand, Suspect>()
             .Map(dest => dest.Id, src => Guid.NewGuid())
             .Map(dest => dest.CreatedAt, src => DateTime.UtcNow)
             .Map(dest => dest.CurrentStatus, src => src.Status)
             .AfterMapping((src, dest) => dest.UpdateFullName());

        config.NewConfig<SuspectCreatedEvent, Suspect>();
        #endregion

        #region Suspect Update
        config.NewConfig<UpdateSuspectCommand, Suspect>()
             .Ignore(dest => dest.Id)
             .Ignore(dest => dest.CreatedAt)
             .Map(dest => dest.CurrentStatus, src => src.Status)
             .Map(dest => dest.MaritalStatus, src => src.MaritalStatus)

             .AfterMapping((src, dest) => dest.UpdateFullName());
        #endregion
    }
}
