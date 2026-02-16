using Microsoft.AspNetCore.Mvc;
using NEXUS.Infrastructure.Common;
using NEXUS.Features.LifeHistoryFeature.Create;
using NEXUS.Features.LifeHistoryFeature.Update;
using NEXUS.Features.LifeHistoryFeature.SoftDelete;
using Wolverine;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/life-histories")]
[Tags("LifeHistories")]
public class LifeHistoryController : ControllerBase
{
    private readonly IMessageBus _bus;

    public LifeHistoryController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [EndpointName("CreateLifeHistory")]
    [EndpointSummary("Add a life history event for a suspect")]
    public async Task<Response<Guid>> Create([FromBody] CreateLifeHistoryCommand command)
    {
        return await _bus.InvokeAsync<Response<Guid>>(command);
    }

    [HttpPut]
    [EndpointName("UpdateLifeHistory")]
    [EndpointSummary("Update a life history event")]
    public async Task<Response<LifeHistoryUpdatedEvent>> Update([FromBody] UpdateLifeHistoryCommand command)
    {
        return await _bus.InvokeAsync<Response<LifeHistoryUpdatedEvent>>(command);
    }

    [HttpDelete("{id:guid}")]
    [EndpointName("SoftDeleteLifeHistory")]
    [EndpointSummary("Delete a life history event")]
    public async Task<Response<LifeHistoryDeletedEvent>> Delete(Guid id)
    {
        return await _bus.InvokeAsync<Response<LifeHistoryDeletedEvent>>(new SoftDeleteLifeHistoryCommand(id));
    }
}
