using NEXUS.Data.Entities;
using NEXUS.Data.Enums;
using NEXUS.Dtos;

namespace NEXUS.Services.SqlSearch;

public interface ISqlSearchEngine
{
    // بحث عن المتهمين
    Task<(List<SuspectSearchResultDto> Results, int TotalCount)> SearchSuspectsAsync(SuspectSearchCriteria criteria);

    // بحث عن القضايا (سيناريو إضافي مهم)
    Task<List<Case>> SearchCasesAsync(string query, CaseStatus? status);
}