using NEXUS.Dtos;
using System.Text.RegularExpressions;

namespace NEXUS.Services.TextAnalysis;

public class TextAnalysisService : ITextAnalysisService
{

    public async Task<AnalysisResult> AnalyzeTextAsync(string text)
    {
        var entities = new List<ExtractedEntity>();

        // 1. استخراج أرقام الهواتف (Regex قوي جداً ودقيق)
        var phoneRegex = new Regex(@"07[3-9]\d{8}");
        foreach (Match match in phoneRegex.Matches(text))
        {
            entities.Add(new ExtractedEntity(match.Value, "Phone", "تم ذكره في النص", 1.0));
        }

        // 2. استخراج الكنى والأسماء (هنا يفضل استخدام AI)
        // محاكاة لنتيجة AI:
        // var aiResponse = await _aiClient.ExtractEntities(text);
        // entities.AddRange(aiResponse);

        return new AnalysisResult(entities, "ملخص آلي: المتهم اعترف بالاتصال...");
    }
}