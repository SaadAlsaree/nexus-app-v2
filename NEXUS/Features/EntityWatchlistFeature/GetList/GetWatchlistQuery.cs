namespace NEXUS.Features.EntityWatchlistFeature.GetList;

public record GetWatchlistQuery(
    string? SearchTerm = null,
    int Page = 1,
    int PageSize = 10
);
