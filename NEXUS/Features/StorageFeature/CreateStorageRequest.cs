using Microsoft.AspNetCore.Http;

namespace NEXUS.Features.StorageFeature;

public record CreateStorageRequest(IFormFile File);
