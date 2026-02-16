using Mapster;
using NEXUS.Data.Entities;
using NEXUS.Extensions;
using NEXUS.Features.OperationFeature.GetById;
using NEXUS.Features.OperationFeature.Update;

namespace NEXUS.Mappings;

public class OperationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Operation, OperationDto>()
            .Map(dest => dest.OperationTypeName, src => src.OperationType.GetDisplayName());

        config.NewConfig<UpdateOperationCommand, Operation>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreatedAt);
    }
}
