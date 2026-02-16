using Microsoft.AspNetCore.Mvc;
using NEXUS.Infrastructure.Common;
using NEXUS.Features.CaseFeature.CreateCase;
using NEXUS.Features.CaseFeature.UpdateCase;
using NEXUS.Features.CaseFeature.SoftDeleteCase;
using NEXUS.Features.CaseFeature.GetByIdCase;
using NEXUS.Features.CaseFeature.GetListCases;
using NEXUS.Features.CaseFeature.AddSupectToCase;
using NEXUS.Features.CaseFeature.RemoveSupectFromCase;
using Wolverine;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Cases")]
public class CasesController : ControllerBase
{
    private readonly IMessageBus _bus;

    public CasesController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [EndpointName("CreateCase")]
    [EndpointSummary("Create a new case")]
    public async Task<Response<Guid>> Create([FromBody] CreateCaseCommand command)
    {
        return await _bus.InvokeAsync<Response<Guid>>(command);
    }

    [HttpPut]
    [EndpointName("UpdateCase")]
    [EndpointSummary("Update an existing case")]
    public async Task<Response<CaseUpdatedEvent>> Update([FromBody] UpdateCaseCommand command)
    {
        return await _bus.InvokeAsync<Response<CaseUpdatedEvent>>(command);
    }

    [HttpDelete("{id:guid}")]
    [EndpointName("SoftDeleteCase")]
    [EndpointSummary("Delete a case (soft delete)")]
    public async Task<Response<CaseDeletedEvent>> Delete(Guid id)
    {
        return await _bus.InvokeAsync<Response<CaseDeletedEvent>>(new SoftDeleteCaseCommand(id));
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetCaseById")]
    [EndpointSummary("Get a case by its unique ID")]
    public async Task<Response<CaseDetailsDto>> GetById(Guid id)
    {
        return await _bus.InvokeAsync<Response<CaseDetailsDto>>(new GetByIdCaseQuery(id));
    }

    [HttpGet]
    [EndpointName("GetListCases")]
    [EndpointSummary("Get all cases")]
    public async Task<Response<PagedList<CaseSummaryDto>>> GetList([FromQuery] GetListCasesQuery query)
    {
        return await _bus.InvokeAsync<Response<PagedList<CaseSummaryDto>>>(query);
    }

    [HttpPost("{caseId:guid}/suspects")]
    [EndpointName("AddSuspectToCase")]
    [EndpointSummary("Add a suspect to a case")]
    public async Task<Response<SuspectAddedToCaseEvent>> AddSuspect(Guid caseId, [FromBody] AddSuspectToCaseCommand command)
    {
        if (command.CaseId != caseId)
        {
            command = command with { CaseId = caseId };
        }
        return await _bus.InvokeAsync<Response<SuspectAddedToCaseEvent>>(command);
    }

    [HttpDelete("{caseId:guid}/suspects/{suspectId:guid}")]
    [EndpointName("RemoveSuspectFromCase")]
    [EndpointSummary("Remove a suspect from a case")]
    public async Task<Response<SuspectRemovedFromCaseEvent>> RemoveSuspect(Guid caseId, Guid suspectId)
    {
        return await _bus.InvokeAsync<Response<SuspectRemovedFromCaseEvent>>(new RemoveSuspectFromCaseCommand(caseId, suspectId));
    }
}
