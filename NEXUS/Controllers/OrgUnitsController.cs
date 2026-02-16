using Microsoft.AspNetCore.Mvc;
using NEXUS.Infrastructure.Common;
using NEXUS.Features.OrgUnitFeature.Create;
using NEXUS.Features.OrgUnitFeature.Update;
using NEXUS.Features.OrgUnitFeature.Delete;
using NEXUS.Features.OrgUnitFeature.GetById;
using NEXUS.Features.OrgUnitFeature.GetList;
using NEXUS.Features.OrgUnitFeature.GetHierarchy;
using Wolverine;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/org-units")]
[Tags("OrgUnits")]
public class OrgUnitsController : ControllerBase
{
    private readonly IMessageBus _bus;

    public OrgUnitsController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [EndpointName("CreateOrgUnit")]
    [EndpointSummary("Create a new organizational unit")]
    public async Task<Response<Guid>> Create([FromBody] CreateOrgUnitCommand command)
    {
        return await _bus.InvokeAsync<Response<Guid>>(command);
    }

    [HttpPut]
    [EndpointName("UpdateOrgUnit")]
    [EndpointSummary("Update an organizational unit")]
    public async Task<Response<OrgUnitUpdatedEvent>> Update([FromBody] UpdateOrgUnitCommand command)
    {
        return await _bus.InvokeAsync<Response<OrgUnitUpdatedEvent>>(command);
    }

    [HttpDelete("{id:guid}")]
    [EndpointName("DeleteOrgUnit")]
    [EndpointSummary("Delete an organizational unit")]
    public async Task<Response<OrgUnitDeletedEvent>> Delete(Guid id)
    {
        return await _bus.InvokeAsync<Response<OrgUnitDeletedEvent>>(new SoftDeleteOrgUnitCommand(id));
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetOrgUnitById")]
    [EndpointSummary("Get an organizational unit by id")]
    public async Task<Response<OrgUnitDetailsDto>> GetById(Guid id)
    {
        return await _bus.InvokeAsync<Response<OrgUnitDetailsDto>>(new GetOrgUnitByIdQuery(id));
    }

    [HttpGet]
    [EndpointName("GetOrgUnits")]
    [EndpointSummary("Get all organizational units")]
    public async Task<Response<PagedList<OrgUnitSummaryDto>>> GetList([FromQuery] GetOrgUnitsQuery query)
    {
        return await _bus.InvokeAsync<Response<PagedList<OrgUnitSummaryDto>>>(query);
    }

    [HttpGet("hierarchy")]
    [EndpointName("GetOrgUnitHierarchy")]
    [EndpointSummary("Get the organizational unit hierarchy")]
    public async Task<Response<List<OrgUnitHierarchyDto>>> GetHierarchy()
    {
        return await _bus.InvokeAsync<Response<List<OrgUnitHierarchyDto>>>(new GetOrgUnitHierarchyQuery());
    }
}
