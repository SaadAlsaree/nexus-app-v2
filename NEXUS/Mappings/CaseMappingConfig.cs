using Mapster;
using NEXUS.Data.Entities;
using NEXUS.Extensions;
using NEXUS.Features.CaseFeature.CreateCase;
using NEXUS.Features.CaseFeature.GetByIdCase;
using NEXUS.Features.CaseFeature.GetListCases;
using NEXUS.Features.CaseFeature.UpdateCase;

namespace NEXUS.Mappings;

public class CaseMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        #region GetListCases
        config.NewConfig<Case, CaseSummaryDto>()
              .Map(dest => dest.StatusName, src => src.Status.GetDisplayName())
              .Map(dest => dest.PriorityName, src => src.Priority.GetDisplayName());
        #endregion

        #region GetByIdCase
        config.NewConfig<Case, CaseDetailsDto>()
              .Map(dest => dest.StatusName, src => src.Status.GetDisplayName())
              .Map(dest => dest.PriorityName, src => src.Priority.GetDisplayName())
              .Map(dest => dest.Suspects, src => src.SuspectsInvolved);

        config.NewConfig<CaseSuspect, CaseSuspectDto>()
              .Map(dest => dest.FullName, src => src.Suspect.FullName)
              .Map(dest => dest.AccusationTypeName, src => src.AccusationType.GetDisplayName())
              .Map(dest => dest.LegalStatusName, src => src.LegalStatus.GetDisplayName());
        #endregion

        #region CreateCase
        config.NewConfig<CreateCaseCommand, Case>()
              .Map(dest => dest.Id, src => Guid.NewGuid())
              .Map(dest => dest.CreatedAt, src => DateTime.UtcNow);
        #endregion

        #region UpdateCase
        config.NewConfig<UpdateCaseCommand, Case>()
              .Ignore(dest => dest.Id)
              .Ignore(dest => dest.CreatedAt);
        #endregion
    }
}
