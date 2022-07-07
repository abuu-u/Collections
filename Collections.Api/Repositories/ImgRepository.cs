using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Collections.Api.Models.Collections;

namespace Collections.Api.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly Cloudinary _cloudinary;

    public ImageRepository(IConfiguration configuration)
    {
        var cloud = configuration["CLOUDINARY_CLOUD"] ??
                    throw new Exception("set CLOUDINARY_CLOUD environment variable");
        var apiKey = configuration["CLOUDINARY_API_KEY"] ??
                     throw new Exception("set CLOUDINARY_API_KEY environment variable");
        var apiSecret = configuration["CLOUDINARY_API_SECRET"] ??
                        throw new Exception("set CLOUDINARY_API_SECRET environment variable");
        _cloudinary = new Cloudinary(new Account(cloud, apiKey, apiSecret));
    }

    public async Task<SaveImageResponse> Upload(SaveImageRequest model)
    {
        var uploadParams = new ImageUploadParams();
        using var memoryStream = new MemoryStream();
        await model.Image.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        uploadParams.File = new FileDescription(model.Image.FileName, memoryStream);
        var result = await _cloudinary.UploadAsync(uploadParams);
        return new SaveImageResponse { ImageUrl = result.SecureUrl.ToString() };
    }
}
