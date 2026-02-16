using Microsoft.AspNetCore.Mvc;
using NEXUS.Infrastructure.Common;
using NEXUS.Features.AddressFeature.CreateSuspectAddres;
using NEXUS.Features.AddressFeature.UpdateSuspectAddres;
using NEXUS.Features.AddressFeature.GetByIdSuspectAddres;
using NEXUS.Features.AddressFeature.GetBySuspectId;
using NEXUS.Features.AddressFeature.Delete;
using Wolverine;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Addresses")]
public class AddressesController : ControllerBase
{
    private readonly IMessageBus _bus;

    public AddressesController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [EndpointName("CreateSuspectAddress")]
    [EndpointSummary("Add a new address for a suspect")]
    public async Task<Response<Guid>> Create([FromBody] CreateSuspectAddressCommand command)
    {
        return await _bus.InvokeAsync<Response<Guid>>(command);
    }

    [HttpPut]
    [EndpointName("UpdateSuspectAddress")]
    [EndpointSummary("Update an existing address")]
    public async Task<Response<SuspectAddressUpdatedEvent>> Update([FromBody] UpdateSuspectAddressCommand command)
    {
        return await _bus.InvokeAsync<Response<SuspectAddressUpdatedEvent>>(command);
    }

    [HttpDelete("{id:guid}")]
    [EndpointName("SoftDeleteAddress")]
    [EndpointSummary("Delete an address")]
    public async Task<Response<AddressDeletedEvent>> Delete(Guid id)
    {
        return await _bus.InvokeAsync<Response<AddressDeletedEvent>>(new SoftDeleteAddressCommand(id));
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetSuspectAddressById")]
    [EndpointSummary("Get an address by its unique ID")]
    public async Task<Response<AddressDto>> GetById(Guid id)
    {
        return await _bus.InvokeAsync<Response<AddressDto>>(new GetByIdSuspectAddressQuery(id));
    }

    [HttpGet("/api/suspects/{suspectId:guid}/addresses")]
    [EndpointName("GetAddressesBySuspectId")]
    [EndpointSummary("Get all addresses for a specific suspect")]
    public async Task<Response<List<AddressDto>>> GetBySuspectId(Guid suspectId)
    {
        return await _bus.InvokeAsync<Response<List<AddressDto>>>(new GetBySuspectIdQuery(suspectId));
    }
}
