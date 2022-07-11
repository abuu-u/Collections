using Collections.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Collections.Api.Controllers;

[ApiController]
[Route("CollectionsApi/[controller]")]
public class TagsController : ControllerBase
{
    private readonly IItemService _itemService;

    public TagsController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMostUsedTags(int count)
    {
        var tags = await _itemService.GetMostUsedTags(count);
        return Ok(tags);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchTags(string? str, int count)
    {
        var tags = await _itemService.SearchTags(str, count);
        return Ok(tags);
    }
}
