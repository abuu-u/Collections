using Collections.Api.Authorization;
using Collections.Api.Entities;
using Collections.Api.Models.Collections;
using Collections.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Collections.Api.Controllers;

[Authorize]
[ApiController]
[Route("CollectionsApi/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemsController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> Search(string? searchString, int page, int count)
    {
        var items = await _itemService.Search(searchString, page, count);
        return Ok(items);
    }

    [HttpPost("{itemId:int}")]
    public async Task<IActionResult> CreateComment(CreateCommentRequest model, int itemId)
    {
        var user = HttpContext.Items["User"] as User;
        var comment = await _itemService.CreateComment(model, itemId, user!);
        return Ok(comment);
    }

    [AllowAnonymous]
    [HttpGet("{itemId:int}/comments")]
    public async Task<IActionResult> GetComments(int itemId)
    {
        var comments = await _itemService.GetComments(itemId);
        return Ok(comments);
    }

    [HttpPost("{itemId:int}/like")]
    public async Task<IActionResult> Like(int itemId)
    {
        var author = HttpContext.Items["User"] as User;
        await _itemService.Like(itemId, author!);
        return Ok(new { message = "Liked successfully" });
    }

    [HttpPost("{itemId:int}/unlike")]
    public async Task<IActionResult> UnLike(int itemId)
    {
        var author = HttpContext.Items["User"] as User;
        await _itemService.Unlike(itemId, author!.Id);
        return Ok(new { message = "Unliked successfully" });
    }

    [AllowAnonymous]
    [HttpGet("{itemId:int}")]
    public async Task<IActionResult> Get(int itemId)
    {
        var user = HttpContext.Items["User"] as User;
        var item = await _itemService.Get(itemId, user?.Id);
        return Ok(item);
    }

    [HttpGet("{itemId:int}/for-edit")]
    public async Task<IActionResult> GetItemForEditing(int itemId)
    {
        var user = HttpContext.Items["User"] as User;
        var item = await _itemService.GetItemForEditing(itemId, user?.Id);
        return Ok(item);
    }

    [AllowAnonymous]
    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestItems(int count)
    {
        var items = await _itemService.GetLatestItems(count);
        return Ok(items);
    }
}
