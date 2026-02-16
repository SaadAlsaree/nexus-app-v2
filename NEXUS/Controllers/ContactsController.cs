using Microsoft.AspNetCore.Mvc;
using NEXUS.Infrastructure.Common;
using NEXUS.Features.ContactFeature.CreateSuspectContact;
using NEXUS.Features.ContactFeature.UpdateSuspectContact;
using NEXUS.Features.ContactFeature.GetByIdSuspectContact;
using NEXUS.Features.ContactFeature.GetBySuspectId;
using Wolverine;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Contacts")]
public class ContactsController : ControllerBase
{
    private readonly IMessageBus _bus;

    public ContactsController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [EndpointName("CreateSuspectContact")]
    [EndpointSummary("Add a new contact for a suspect")]
    public async Task<Response<Guid>> Create([FromBody] CreateSuspectContactCommand command)
    {
        return await _bus.InvokeAsync<Response<Guid>>(command);
    }

    [HttpPut]
    [EndpointName("UpdateSuspectContact")]
    [EndpointSummary("Update an existing contact")]
    public async Task<Response<SuspectContactUpdatedEvent>> Update([FromBody] UpdateSuspectContactCommand command)
    {
        return await _bus.InvokeAsync<Response<SuspectContactUpdatedEvent>>(command);
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetSuspectContactById")]
    [EndpointSummary("Get a contact by its unique ID")]
    public async Task<Response<ContactDto>> GetById(Guid id)
    {
        return await _bus.InvokeAsync<Response<ContactDto>>(new GetByIdSuspectContactQuery(id));
    }

    [HttpGet("/api/suspects/{suspectId:guid}/contacts")]
    [EndpointName("GetContactsBySuspectId")]
    [EndpointSummary("Get all contacts for a specific suspect")]
    public async Task<Response<List<ContactDto>>> GetBySuspectId(Guid suspectId)
    {
        return await _bus.InvokeAsync<Response<List<ContactDto>>>(new GetBySuspectIdQuery(suspectId));
    }
}
