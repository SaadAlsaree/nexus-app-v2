using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Common;

namespace NEXUS.Features.CaseFeature.CreateCase;

public sealed class CreateCaseHandler(AppDbContext _db)
{
    public async Task<(Response<Guid>, CaseCreatedEvent)> Handle(CreateCaseCommand command)
    {
        var @case = new Case
        {
            Id = Guid.NewGuid(),
            CaseFileNumber = command.CaseFileNumber,
            Title = command.Title,
            OpenDate = command.OpenDate,
            Status = command.Status,
            InvestigatingOfficer = command.InvestigatingOfficer,
            Priority = command.Priority,
            CreatedAt = DateTime.UtcNow
        };

        _db.Cases.Add(@case);
        await _db.SaveChangesAsync();

        return (Response<Guid>.Success(@case.Id), new CaseCreatedEvent(@case.Id, @case.CaseFileNumber));
    }
}
