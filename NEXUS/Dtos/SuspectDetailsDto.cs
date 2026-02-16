namespace NEXUS.Dtos;

public record SuspectDetailsDto
{
    // رقم القضية (سيتحول إلى عقدة Case)
    public string? CaseNumber { get; init; }

    // المحافظة/الولاية (سيتحول إلى عقدة Location)
    public string? Governorate { get; init; }

    // قائمة الأحداث: دورات، عمليات، سجن سابق
    // (ستتحول إلى عقد Event وعلاقات PARTICIPATED_IN)
    public List<HistoryEventDto> HistoryEvents { get; init; } = new();
}

public record HistoryEventDto(string Name, string Type);