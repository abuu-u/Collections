using Collections.Api.Authorization;
using Collections.Api.Models.Collections;
using Collections.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Collections.Api.Controllers;

[Authorize]
[ApiController]
[Route("collections/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImagesController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm]SaveImageRequest model)
    {
        var result = await _imageService.Upload(model);
        return Ok(result);
    }
}