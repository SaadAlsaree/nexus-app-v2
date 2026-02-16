using Mapster;
using NEXUS.Data.Entities;
using NEXUS.Extensions;
using NEXUS.Features.InterrogationSessionFeature.GetById;
using NEXUS.Features.InterrogationSessionFeature.GetList;
using NEXUS.Features.InterrogationSessionFeature.Update;

namespace NEXUS.Mappings;

public class InterrogationSessionMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<InterrogationSession, InterrogationSessionDto>()
            .Map(dest => dest.SessionTypeName, src => src.SessionType.GetDisplayName())
            .Map(dest => dest.OutcomeName, src => src.Outcome.GetDisplayName())
            .Map(dest => dest.Suspect, src => src.Suspect != null ? new SuspectDto(
                src.Suspect.Id,
                src.Suspect.FirstName,
                src.Suspect.SecondName,
                src.Suspect.ThirdName,
                src.Suspect.FourthName,
                src.Suspect.FivthName,
                src.Suspect.FullName,
                src.Suspect.Kunya,
                src.Suspect.MotherName,
                src.Suspect.DateOfBirth,
                src.Suspect.PlaceOfBirth,
                src.Suspect.Tribe,
                src.Suspect.MaritalStatus,
                src.Suspect.MaritalStatus.HasValue ? src.Suspect.MaritalStatus.Value.GetDisplayName() : null,
                src.Suspect.WivesCount,
                src.Suspect.ChildrenCount,
                src.Suspect.LegalArticle,
                src.Suspect.HealthStatus,
                src.Suspect.PhotoUrl,
                src.Suspect.CurrentStatus,
                src.Suspect.CurrentStatus.GetDisplayName(),
                src.Suspect.CreatedAt
            ) : null!);

        config.NewConfig<InterrogationSession, InterrogationSessionSummaryDto>()
            .Map(dest => dest.SuspectFullName, src => src.Suspect.FullName)
            .Map(dest => dest.CaseTitle, src => src.Case.Title)
            .Map(dest => dest.SessionTypeName, src => src.SessionType.GetDisplayName())
            .Map(dest => dest.OutcomeName, src => src.Outcome.GetDisplayName());

        config.NewConfig<UpdateInterrogationSessionCommand, InterrogationSession>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt);
    }
}
