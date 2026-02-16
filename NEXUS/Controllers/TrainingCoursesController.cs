using Microsoft.AspNetCore.Mvc;
using NEXUS.Infrastructure.Common;
using NEXUS.Features.TrainingCourseFeature.Create;
using NEXUS.Features.TrainingCourseFeature.Update;
using NEXUS.Features.TrainingCourseFeature.Delete;
using NEXUS.Features.TrainingCourseFeature.GetById;
using NEXUS.Features.TrainingCourseFeature.GetBySuspectId;
using Wolverine;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/training-courses")]
[Tags("TrainingCourses")]
public class TrainingCoursesController : ControllerBase
{
    private readonly IMessageBus _bus;

    public TrainingCoursesController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    [EndpointName("CreateTrainingCourse")]
    [EndpointSummary("Create a new training course")]
    public async Task<Response<Guid>> Create([FromBody] CreateTrainingCourseCommand command)
    {
        return await _bus.InvokeAsync<Response<Guid>>(command);
    }

    [HttpPut]
    [EndpointName("UpdateTrainingCourse")]
    [EndpointSummary("Update a training course")]
    public async Task<Response<TrainingCourseUpdatedEvent>> Update([FromBody] UpdateTrainingCourseCommand command)
    {
        return await _bus.InvokeAsync<Response<TrainingCourseUpdatedEvent>>(command);
    }

    [HttpDelete("{id:guid}")]
    [EndpointName("SoftDeleteTrainingCourse")]
    [EndpointSummary("Delete a training course")]
    public async Task<Response<TrainingCourseDeletedEvent>> Delete(Guid id)
    {
        return await _bus.InvokeAsync<Response<TrainingCourseDeletedEvent>>(new SoftDeleteTrainingCourseCommand(id));
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetTrainingCourseById")]
    [EndpointSummary("Get a training course by id")]
    public async Task<Response<TrainingCourseDto>> GetById(Guid id)
    {
        return await _bus.InvokeAsync<Response<TrainingCourseDto>>(new GetTrainingCourseByIdQuery(id));
    }

    [HttpGet("/api/suspects/{suspectId:guid}/training-courses")]
    [EndpointName("GetTrainingCoursesBySuspectId")]
    [EndpointSummary("Get all training courses for a specific suspect")]
    public async Task<Response<List<TrainingCourseDto>>> GetBySuspectId(Guid suspectId)
    {
        return await _bus.InvokeAsync<Response<List<TrainingCourseDto>>>(new GetTrainingCoursesBySuspectIdQuery(suspectId));
    }
}
