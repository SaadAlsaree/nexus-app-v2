using Microsoft.AspNetCore.Mvc;
using NEXUS.Infrastructure.Common;
using NEXUS.Features.EvidenceAttachmentFeature.Create;
using NEXUS.Features.EvidenceAttachmentFeature.Update;
using NEXUS.Features.EvidenceAttachmentFeature.GetById;
using NEXUS.Features.EvidenceAttachmentFeature.GetList;
using NEXUS.Features.EvidenceAttachmentFeature.GetByCaseId;
using NEXUS.Features.EvidenceAttachmentFeature.GetBySuspectId;
using NEXUS.Features.EvidenceAttachmentFeature.SoftDelete;
using Wolverine;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/evidence")]
[Tags("Evidence")]
public class EvidenceController(IMessageBus bus) : ControllerBase
{
    [HttpPost]
    [EndpointName("CreateEvidence")]
    [EndpointSummary("Upload a new evidence attachment")]
    public async Task<IResult> Create([FromForm] CreateEvidenceCommand command)
    {
        var result = await bus.InvokeAsync<Result<Guid>>(command);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
    }

    [HttpGet]
    [EndpointName("GetEvidenceList")]
    [EndpointSummary("Get all evidence attachments")]
    public async Task<IEnumerable<EvidenceDto>> GetList([FromQuery] GetEvidenceListQuery query)
    {
        return await bus.InvokeAsync<IEnumerable<EvidenceDto>>(query);
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetEvidenceById")]
    [EndpointSummary("Get evidence attachment by id")]
    public async Task<IResult> GetById(Guid id)
    {
        var result = await bus.InvokeAsync<Result<EvidenceDto>>(new GetEvidenceByIdQuery(id));
        if (result.IsFailure)
        {
            return result.Error.Type == ErrorType.NotFound
                ? Results.NotFound(result.Error)
                : Results.BadRequest(result.Error);
        }
        return Results.Ok(result.Value);
    }

    [HttpGet("case/{caseId:guid}")]
    [EndpointName("GetEvidenceByCaseId")]
    [EndpointSummary("Get evidence attachments for a specific case")]
    public async Task<IEnumerable<EvidenceDto>> GetByCaseId(Guid caseId)
    {
        return await bus.InvokeAsync<IEnumerable<EvidenceDto>>(new GetEvidenceByCaseIdQuery(caseId));
    }

    [HttpGet("suspect/{suspectId:guid}")]
    [EndpointName("GetEvidenceBySuspectId")]
    [EndpointSummary("Get evidence attachments for a specific suspect")]
    public async Task<IEnumerable<EvidenceDto>> GetBySuspectId(Guid suspectId)
    {
        return await bus.InvokeAsync<IEnumerable<EvidenceDto>>(new GetEvidenceBySuspectIdQuery(suspectId));
    }

    [HttpPut("{id:guid}")]
    [EndpointName("UpdateEvidence")]
    [EndpointSummary("Update evidence attachment details")]
    public async Task<IResult> Update(Guid id, [FromBody] UpdateEvidenceCommand command)
    {
        if (command.Id != id)
        {
            command = command with { Id = id };
        }
        var result = await bus.InvokeAsync<Result>(command);
        if (result.IsFailure)
        {
            return result.Error.Type == ErrorType.NotFound
                ? Results.NotFound(result.Error)
                : Results.BadRequest(result.Error);
        }
        return Results.NoContent();
    }

    [HttpDelete("{id:guid}")]
    [EndpointName("DeleteEvidence")]
    [EndpointSummary("Soft delete an evidence attachment")]
    public async Task<IResult> Delete(Guid id)
    {
        var result = await bus.InvokeAsync<Result>(new SoftDeleteEvidenceCommand(id));
        if (result.IsFailure)
        {
            return result.Error.Type == ErrorType.NotFound
                ? Results.NotFound(result.Error)
                : Results.BadRequest(result.Error);
        }
        return Results.NoContent();
    }
}
