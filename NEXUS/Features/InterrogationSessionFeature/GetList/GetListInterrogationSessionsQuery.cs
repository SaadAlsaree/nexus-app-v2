namespace NEXUS.Features.InterrogationSessionFeature.GetList;

public record GetListInterrogationSessionsQuery(
    string? SearchTerm = null,
    int Page = 1,
    int PageSize = 10
);
