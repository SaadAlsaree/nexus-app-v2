using Microsoft.AspNetCore.Http;

namespace NEXUS.Features.SuspectFeature.UploadPhoto;

public record UploadPhotoRequest(IFormFile File);
