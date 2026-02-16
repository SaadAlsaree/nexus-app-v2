using Microsoft.AspNetCore.Mvc;
using NEXUS.Infrastructure.Common;
using NEXUS.Features.OperationFeature.Create;
using NEXUS.Features.OperationFeature.Update;
using NEXUS.Features.OperationFeature.Delete;
using NEXUS.Features.OperationFeature.GetById;
using NEXUS.Features.OperationFeature.GetBySuspectId;
using Wolverine;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Operations")]
public class OperationsController : ControllerBase
{
    private readonly IMessageBus _bus;

    public OperationsController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [EndpointName("CreateOperation")]
    [EndpointSummary("Create a new operation")]
    public async Task<Response<Guid>> Create([FromBody] CreateOperationCommand command)
    {
        return await _bus.InvokeAsync<Response<Guid>>(command);
    }

    [HttpPut]
    [EndpointName("UpdateOperation")]
    [EndpointSummary("Update an operation")]
    public async Task<Response<OperationUpdatedEvent>> Update([FromBody] UpdateOperationCommand command)
    {
        return await _bus.InvokeAsync<Response<OperationUpdatedEvent>>(command);
    }

    [HttpDelete("{id:guid}")]
    [EndpointName("SoftDeleteOperation")]
    [EndpointSummary("Delete an operation")]
    public async Task<Response<OperationDeletedEvent>> Delete(Guid id)
    {
        return await _bus.InvokeAsync<Response<OperationDeletedEvent>>(new SoftDeleteOperationCommand(id));
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetOperationById")]
    [EndpointSummary("Get an operation by id")]
    public async Task<Response<OperationDto>> GetById(Guid id)
    {
        return await _bus.InvokeAsync<Response<OperationDto>>(new GetOperationByIdQuery(id));
    }

    [HttpGet("/api/suspects/{suspectId:guid}/operations")]
    [EndpointName("GetOperationsBySuspectId")]
    [EndpointSummary("Get all operations for a specific suspect")]
    public async Task<Response<List<OperationDto>>> GetBySuspectId(Guid suspectId)
    {
        return await _bus.InvokeAsync<Response<List<OperationDto>>>(new GetOperationsBySuspectIdQuery(suspectId));
    }
}
