using NEXUS.Features.SuspectFeature.GetById;
using NEXUS.Features.InterrogationSessionFeature.GetById;
using NEXUS.Services;
using QuestPDF.Infrastructure;

using NEXUS.Infrastructure.Reports.Documents;
using QuestPDF.Fluent;

namespace NEXUS.Infrastructure.Reports;

public class QuestPdfService : IPdfService
{
    public byte[] GenerateSuspectReport(GetByIdDto data)
    {
        var document = new GenerateSuspectReportDocuments(data);
        return document.GeneratePdf();
    }

    public byte[] GenerateInterrogationSessionReport(InterrogationSessionDto data)
    {
        var document = new GenerateInterrogationSessionReportDocuments(data);
        return document.GeneratePdf();
    }
}