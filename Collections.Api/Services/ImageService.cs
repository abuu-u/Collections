using Collections.Api.Models.Collections;
using Collections.Api.Repositories;

namespace Collections.Api.Services;

public interface IImageService
{
    Task<SaveImageResponse> Upload(SaveImageRequest model);
}

public class ImageService: IImageService
{
    private readonly IImageRepository _imageRepository;

    public ImageService(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<SaveImageResponse> Upload(SaveImageRequest model)
    {
        return await _imageRepository.Upload(model);
    }
}