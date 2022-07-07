using Collections.Api.Models.Collections;
using Collections.Api.Repositories;

namespace Collections.Api.Services;

public interface IImageService
{
    Task<SaveImageResponse> SaveImage(SaveImageRequest model);
}

public class ImageService: IImageService
{
    private readonly IImageRepository _ImageRepository;

    public ImageService(IImageRepository ImageRepository)
    {
        _ImageRepository = ImageRepository;
    }

    public async Task<SaveImageResponse> SaveImage(SaveImageRequest model)
    {
        return await _ImageRepository.Upload(model);
    }
}