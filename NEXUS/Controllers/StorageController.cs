using Microsoft.AspNetCore.Mvc;
using NEXUS.Events;
using NEXUS.Infrastructure.Common;
using NEXUS.Services.Storage;
using NEXUS.Features.StorageFeature;

namespace NEXUS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Storage")]
public class StorageController : ControllerBase
{
    private readonly IBlobService _blobService;

    public StorageController(IBlobService blobService)
    {
        _blobService = blobService;
    }

    [HttpPost]
    [EndpointName("CreateStorage")]
    [EndpointSummary("Create a new storage")]
    public async Task<IResult> Create([FromForm] CreateStorageRequest request)
    {
        using Stream stream = request.File.OpenReadStream();
        var result = await _blobService.UploadAsync(stream, request.File.ContentType);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return Results.BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    [EndpointName("GetFile")]
    [EndpointSummary("Get a file by id")]
    public async Task<IResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _blobService.DownloadAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return Results.NotFound(result.Error);
        }

        return Results.Stream(result.Value.Stream, result.Value.ContentType);
    }
}
