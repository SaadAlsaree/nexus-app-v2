using Microsoft.AspNetCore.Mvc;
using NEXUS.Infrastructure.Common;
using NEXUS.Features.OrgAssignmentFeature.Add;
using NEXUS.Features.OrgAssignmentFeature.Remove;
using Wolverine;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/org-assignments")]
[Tags("OrgAssignments")]
public class OrgAssignmentsController : ControllerBase
{
    private readonly IMessageBus _bus;

    public OrgAssignmentsController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [EndpointName("AddOrgAssignment")]
    [EndpointSummary("Assign a suspect to an organizational unit")]
    public async Task<Response<Guid>> Create([FromBody] AddOrgAssignmentCommand command)
    {
        return await _bus.InvokeAsync<Response<Guid>>(command);
    }

    [HttpDelete("{id:guid}")]
    [EndpointName("RemoveOrgAssignment")]
    [EndpointSummary("Remove a suspect from an organizational unit")]
    public async Task<Response<OrgAssignmentRemovedEvent>> Delete(Guid id)
    {
        return await _bus.InvokeAsync<Response<OrgAssignmentRemovedEvent>>(new RemoveOrgAssignmentCommand(id));
    }
}
