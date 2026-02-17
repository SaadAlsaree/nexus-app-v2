using Mapster;
using NEXUS.Data.Entities;
using NEXUS.Features.EvidenceAttachmentFeature.GetList;
using NEXUS.Features.EvidenceAttachmentFeature.Update;

namespace NEXUS.Mappings;

public class EvidenceAttachmentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<EvidenceAttachment, EvidenceDto>()
            .Map(dest => dest.DownloadUrl, src =>
                MapContext.Current != null && MapContext.Current.Parameters.ContainsKey("BaseUrl")
                ? $"{MapContext.Current.Parameters["BaseUrl"]}{src.FilePath}"
                : $"/api/storage/{src.FilePath}");

        config.NewConfig<UpdateEvidenceCommand, EvidenceAttachment>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt);
    }
}
