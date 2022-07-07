using Collections.Api.Models.Collections;

namespace Collections.Api.Repositories;

public interface IImageRepository
{
    Task<SaveImageResponse> Upload(SaveImageRequest model);
}