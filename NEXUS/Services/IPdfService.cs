using NEXUS.Features.SuspectFeature.GetById;
using NEXUS.Features.InterrogationSessionFeature.GetById;

namespace NEXUS.Services;

public interface IPdfService
{
    byte[] GenerateSuspectReport(GetByIdDto data);
    byte[] GenerateInterrogationSessionReport(InterrogationSessionDto data);
}