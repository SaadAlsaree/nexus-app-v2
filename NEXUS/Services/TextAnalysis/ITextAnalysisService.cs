using NEXUS.Dtos;

namespace NEXUS.Services.TextAnalysis;

public interface ITextAnalysisService
{
    Task<AnalysisResult> AnalyzeTextAsync(string text);
}