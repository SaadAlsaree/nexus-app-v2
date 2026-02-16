using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.SuspectFeature.SuspectList;

public record GetSuspectListQuery(int? Page = 1, int? PageSize = 10, string? SearchTerm = null);
