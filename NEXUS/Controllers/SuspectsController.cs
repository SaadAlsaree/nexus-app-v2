using Microsoft.AspNetCore.Mvc;
using NEXUS.Data;
using NEXUS.Infrastructure.Common;
using NEXUS.Services.Storage;
using NEXUS.Features.SuspectFeature.Create;
using NEXUS.Features.SuspectFeature.SuspectList;
using NEXUS.Features.SuspectFeature.GetById;
using NEXUS.Features.SuspectFeature.Update;
using NEXUS.Features.SuspectFeature.UpdateStatus;
using NEXUS.Features.SuspectFeature.UploadPhoto;
using NEXUS.Features.SuspectFeature.GetNetwork;
using NEXUS.Features.SuspectFeature.AddRelationship;
using NEXUS.Features.SuspectFeature.UpdateRelationship;
using NEXUS.Features.SuspectFeature.DeleteRelationship;
using NEXUS.Services;
using Wolverine;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Suspects")]
public class SuspectsController : ControllerBase
{
    private readonly IMessageBus _bus;

    public SuspectsController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [EndpointName("CreateSuspect")]
    [EndpointSummary("Create a new suspect")]
    [EndpointDescription("Creates a new suspect in the system")]
    public async Task<Response<Guid>> Create([FromBody] CreateSuspectCommand command)
    {
        return await _bus.InvokeAsync<Response<Guid>>(command);
    }

    [HttpGet]
    [EndpointName("GetSuspectList")]
    [EndpointSummary("Get all suspects")]
    [EndpointDescription("Returns a list of all suspects in the system")]
    public async Task<Response<PagedList<SuspectListDto>>> GetList([FromQuery] GetSuspectListQuery query)
    {
        return await _bus.InvokeAsync<Response<PagedList<SuspectListDto>>>(query);
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetSuspectById")]
    [EndpointSummary("Get suspect by id")]
    [EndpointDescription("Returns a single suspect by its unique identifier")]
    public async Task<Response<GetByIdDto>> GetById([FromRoute] Guid id)
    {
        return await _bus.InvokeAsync<Response<GetByIdDto>>(new GetByIdQuery(id));
    }

    [HttpPut("{suspectId:guid}")]
    [EndpointName("UpdateSuspect")]
    [EndpointSummary("Update suspect details")]
    [EndpointDescription("Updates general information of a suspect")]
    public async Task<Response<SuspectUpdatedEvent>> Update(Guid suspectId, [FromBody] UpdateSuspectCommand command)
    {
        if (command.SuspectId != suspectId)
        {
            command = command with { SuspectId = suspectId };
        }
        return await _bus.InvokeAsync<Response<SuspectUpdatedEvent>>(command);
    }

    [HttpPut("{suspectId:guid}/status")]
    [EndpointName("UpdateSuspectStatus")]
    [EndpointSummary("Update the status of a suspect")]
    [EndpointDescription("Updates the status of a suspect in the system")]
    public async Task<Response<SuspectStatusUpdatedEvent>> UpdateStatus(Guid suspectId, [FromBody] UpdateSuspectStatusCommand command)
    {
        if (command.SuspectId != suspectId)
        {
            command = command with { SuspectId = suspectId };
        }
        await _bus.InvokeAsync(command);
        return Response<SuspectStatusUpdatedEvent>.Success();
    }

    [HttpPost("{suspectId:guid}/photo")]
    [EndpointName("UploadSuspectPhoto")]
    [EndpointSummary("Upload a photo for a suspect")]
    public async Task<IResult> UploadPhoto(
        [FromRoute] Guid suspectId,
        [FromForm] UploadPhotoRequest request,
        [FromServices] AppDbContext dbContext,
        [FromServices] IBlobService blobService,
        CancellationToken cancellationToken)
    {
        var suspect = await dbContext.Suspects.FindAsync([suspectId], cancellationToken);

        if (suspect is null)
        {
            return Results.NotFound(Error.NotFound("Suspect.NotFound", "The suspect with the specified ID was not found."));
        }

        using Stream stream = request.File.OpenReadStream();
        stream.Position = 0;
        var result = await blobService.UploadAsync(stream, request.File.ContentType, cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        suspect.PhotoUrl = result.Value.ToString();
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(result.Value);
    }

    [HttpGet("{suspectId:guid}/network")]
    [EndpointName("GetSuspectNetwork")]
    [EndpointSummary("Get the network of a suspect")]
    public async Task<Response<NetworkGraphDto>> GetNetwork(Guid suspectId)
    {
        return await _bus.InvokeAsync<Response<NetworkGraphDto>>(new GetSuspectNetworkQuery(suspectId));
    }

    [HttpPost("{suspectId:guid}/rescan")]
    [Tags("Suspects")]
    [EndpointName("RescanSuspect")]
    [EndpointSummary("Rescan a suspect's network")]
    public async Task<IResult> Rescan(Guid suspectId, [FromServices] IIntelligenceEngine engine)
    {
        await engine.InferRelationshipsAsync(suspectId);
        return Results.Ok(new { message = "Intelligence scan completed successfully." });
    }

    [HttpGet("{id:guid}/report")]
    [EndpointName("GetSuspectReport")]
    [EndpointSummary("Generate a PDF report for a suspect")]
    public async Task<IActionResult> GetReport(Guid id, [FromServices] IPdfService pdfService)
    {
        var result = await _bus.InvokeAsync<Response<GetByIdDto>>(new GetByIdQuery(id));
        if (!result.Succeeded || result.Data == null)
            return NotFound(new { result.Code, result.Message });

        var pdf = pdfService.GenerateSuspectReport(result.Data);
        return File(pdf, "application/pdf", $"SuspectReport_{id}.pdf");
    }

    // Relationship endpoints (also tagged with "Relationships" in Swagger if needed via controller attributes or action attributes)

    [HttpPost("{suspectId:guid}/relationships")]
    [Tags("Suspects", "Relationships")]
    [EndpointName("AddSuspectRelationship")]
    [EndpointSummary("Add a relationship to a suspect")]
    [EndpointDescription("Creates a new relationship between a suspect and another person")]
    public async Task<Response<RelationshipAddedEvent>> AddRelationship(Guid suspectId, [FromBody] AddRelatedPersonCommand command)
    {
        if (command.SuspectId != suspectId)
        {
            command = command with { SuspectId = suspectId };
        }
        return await _bus.InvokeAsync<Response<RelationshipAddedEvent>>(command);
    }

    [HttpPut("{suspectId:guid}/relationships/{id:guid}")]
    [Tags("Suspects", "Relationships")]
    [EndpointName("UpdateSuspectRelationship")]
    [EndpointSummary("Update a relationship for a suspect")]
    [EndpointDescription("Updates an existing relationship between a suspect and another person")]
    public async Task<Response<RelationshipUpdatedEvent>> UpdateRelationship(Guid suspectId, Guid id, [FromBody] UpdateRelatedPersonCommand command)
    {
        if (command.Id != id)
        {
            command = command with { Id = id };
        }
        return await _bus.InvokeAsync<Response<RelationshipUpdatedEvent>>(command);
    }

    [HttpDelete("{suspectId:guid}/relationships/{id:guid}")]
    [Tags("Suspects", "Relationships")]
    [EndpointName("DeleteSuspectRelationship")]
    [EndpointSummary("Delete a relationship for a suspect")]
    [EndpointDescription("Deletes an existing relationship between a suspect and another person")]
    public async Task<Response<RelationshipDeletedEvent>> DeleteRelationship(Guid suspectId, Guid id)
    {
        return await _bus.InvokeAsync<Response<RelationshipDeletedEvent>>(new DeleteRelatedPersonCommand(id, suspectId));
    }
}
