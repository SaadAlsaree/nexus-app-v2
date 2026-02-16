using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.SuspectFeature.GetById;

public class GetByIdHandler
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _contextAccessor;

    public GetByIdHandler(AppDbContext db, IMapper mapper, IHttpContextAccessor contextAccessor)
    {
        _db = db;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
    }

    public async Task<Response<GetByIdDto>> Handle(
        GetByIdQuery query,
        CancellationToken cancellationToken)
    {

        var suspect = await _db.Suspects
            .AsNoTracking()
            .AsSplitQuery()
            .Include(s => s.Addresses)
            .Include(s => s.Contacts)
            .Include(s => s.RelativesAndAssociates)
            .Include(s => s.LifeHistories)
            .Include(s => s.BayahDetails)
            .Include(s => s.TrainingCourses)
            .Include(s => s.Operations)
            .Include(s => s.OrganizationalAssignments)
                .ThenInclude(a => a.OrgUnit)
            .Include(s => s.OrganizationalAssignments)
                .ThenInclude(a => a.DirectCommander)
            .Include(s => s.CaseInvolvements)
                .ThenInclude(ci => ci.Case)
            .FirstOrDefaultAsync(s => s.Id == query.SuspectId && !s.IsDeleted, cancellationToken);

        if (suspect == null)
        {
            return Response<GetByIdDto>.Failure(Error.NotFound("Suspect.NotFound", "Suspect not found"));
        }

        var baseUrl = $"{_contextAccessor.HttpContext?.Request.Scheme}://{_contextAccessor.HttpContext?.Request.Host}";

        var dto = _mapper.From(suspect)
                         .AddParameters("BaseUrl", baseUrl)
                         .AdaptToType<GetByIdDto>();

        return Response<GetByIdDto>.Success(dto);
    }
}