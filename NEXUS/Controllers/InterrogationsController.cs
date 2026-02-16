using Microsoft.AspNetCore.Mvc;
using NEXUS.Infrastructure.Common;
using NEXUS.Features.InterrogationSessionFeature.Create;
using NEXUS.Features.InterrogationSessionFeature.Update;
using NEXUS.Features.InterrogationSessionFeature.Delete;
using NEXUS.Features.InterrogationSessionFeature.GetById;
using NEXUS.Features.InterrogationSessionFeature.GetList;
using NEXUS.Features.InterrogationSessionFeature.GetBySuspectId;
using NEXUS.Services;
using Wolverine;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/interrogation-sessions")]
[Tags("InterrogationSessions")]
public class InterrogationsController : ControllerBase
{
    private readonly IMessageBus _bus;

    public InterrogationsController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [EndpointName("CreateInterrogationSession")]
    [EndpointSummary("Create a new interrogation session")]
    public async Task<Response<Guid>> Create([FromBody] CreateInterrogationSessionCommand command)
    {
        return await _bus.InvokeAsync<Response<Guid>>(command);
    }

    [HttpPut]
    [EndpointName("UpdateInterrogationSession")]
    [EndpointSummary("Update an interrogation session")]
    public async Task<Response<InterrogationSessionUpdatedEvent>> Update([FromBody] UpdateInterrogationSessionCommand command)
    {
        return await _bus.InvokeAsync<Response<InterrogationSessionUpdatedEvent>>(command);
    }

    [HttpDelete("{id:guid}")]
    [EndpointName("SoftDeleteInterrogationSession")]
    [EndpointSummary("Delete an interrogation session")]
    public async Task<Response<InterrogationSessionDeletedEvent>> Delete(Guid id)
    {
        return await _bus.InvokeAsync<Response<InterrogationSessionDeletedEvent>>(new SoftDeleteInterrogationSessionCommand(id));
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetInterrogationSessionById")]
    [EndpointSummary("Get an interrogation session by id")]
    public async Task<Response<InterrogationSessionDto>> GetById(Guid id)
    {
        return await _bus.InvokeAsync<Response<InterrogationSessionDto>>(new GetInterrogationSessionByIdQuery(id));
    }

    [HttpGet]
    [EndpointName("GetListInterrogationSessions")]
    [EndpointSummary("Get all interrogation sessions")]
    public async Task<Response<PagedList<InterrogationSessionSummaryDto>>> GetList([FromQuery] GetListInterrogationSessionsQuery query)
    {
        return await _bus.InvokeAsync<Response<PagedList<InterrogationSessionSummaryDto>>>(query);
    }

    [HttpGet("/api/suspects/{suspectId:guid}/interrogation-sessions")]
    [EndpointName("GetInterrogationSessionsBySuspectId")]
    [EndpointSummary("Get all interrogation sessions for a specific suspect")]
    public async Task<Response<List<InterrogationSessionDto>>> GetBySuspectId(Guid suspectId)
    {
        return await _bus.InvokeAsync<Response<List<InterrogationSessionDto>>>(new GetInterrogationSessionsBySuspectIdQuery(suspectId));
    }

    [HttpGet("{id:guid}/report")]
    [EndpointName("GetInterrogationSessionReport")]
    [EndpointSummary("Generate a PDF report for an interrogation session")]
    public async Task<IActionResult> GetReport(Guid id, [FromServices] IPdfService pdfService)
    {
        var result = await _bus.InvokeAsync<Response<InterrogationSessionDto>>(new GetInterrogationSessionByIdQuery(id));
        if (!result.Succeeded || result.Data == null)
            return NotFound(new { result.Code, result.Message });

        var pdf = pdfService.GenerateInterrogationSessionReport(result.Data);
        return File(pdf, "application/pdf", $"InterrogationReport_{id}.pdf");
    }
}
