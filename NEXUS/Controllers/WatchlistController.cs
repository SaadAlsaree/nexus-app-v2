using Microsoft.AspNetCore.Mvc;
using NEXUS.Infrastructure.Common;
using NEXUS.Features.EntityWatchlistFeature.Create;
using NEXUS.Features.EntityWatchlistFeature.Update;
using NEXUS.Features.EntityWatchlistFeature.GetById;
using NEXUS.Features.EntityWatchlistFeature.GetList;
using NEXUS.Features.EntityWatchlistFeature.Delete;
using Wolverine;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Watchlist")]
public class WatchlistController(IMessageBus bus) : ControllerBase
{
    [HttpPost]
    [EndpointName("CreateWatchlistEntry")]
    [EndpointSummary("Create a new watchlist entry")]
    public async Task<Response<WatchlistEntryCreatedEvent>> Create([FromBody] CreateWatchlistEntryCommand command)
    {
        return await bus.InvokeAsync<Response<WatchlistEntryCreatedEvent>>(command);
    }

    [HttpGet]
    [EndpointName("GetWatchlist")]
    [EndpointSummary("Get the entity watchlist")]
    public async Task<Response<List<WatchlistEntryDto>>> GetList([FromQuery] GetWatchlistQuery query)
    {
        return await bus.InvokeAsync<Response<List<WatchlistEntryDto>>>(query);
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetWatchlistEntryById")]
    [EndpointSummary("Get a watchlist entry by id")]
    public async Task<Response<WatchlistEntryDto>> GetById(Guid id)
    {
        return await bus.InvokeAsync<Response<WatchlistEntryDto>>(new GetWatchlistEntryByIdQuery(id));
    }

    [HttpPut("{id:guid}")]
    [EndpointName("UpdateWatchlistEntry")]
    [EndpointSummary("Update a watchlist entry")]
    public async Task<Response<WatchlistEntryUpdatedEvent>> Update(Guid id, [FromBody] UpdateWatchlistEntryCommand command)
    {
        if (command.Id != id)
        {
            command = command with { Id = id };
        }
        return await bus.InvokeAsync<Response<WatchlistEntryUpdatedEvent>>(command);
    }

    [HttpDelete("{id:guid}")]
    [EndpointName("DeleteWatchlistEntry")]
    [EndpointSummary("Soft delete a watchlist entry")]
    public async Task<Response<WatchlistEntryDeletedEvent>> Delete(Guid id)
    {
        return await bus.InvokeAsync<Response<WatchlistEntryDeletedEvent>>(new SoftDeleteWatchlistEntryCommand(id));
    }
}
