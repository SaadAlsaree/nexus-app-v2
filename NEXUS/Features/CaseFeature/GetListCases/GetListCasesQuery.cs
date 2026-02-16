namespace NEXUS.Features.CaseFeature.GetListCases;

public record GetListCasesQuery(
    string? SearchTerm = null,
    int? Page = 1,
    int? PageSize = 10
);
