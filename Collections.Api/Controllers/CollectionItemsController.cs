using Collections.Api.Authorization;
using Collections.Api.Entities;
using Collections.Api.Models.Collections;
using Collections.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Collections.Api.Controllers;

[Authorize]
[ApiController]
[Route("CollectionsApi/Collections/{collectionId:int}/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly ICollectionService _collectionService;

    private readonly IItemService _itemService;

    public ItemsController(ICollectionService collectionService, IItemService itemService)
    {
        _collectionService = collectionService;
        _itemService = itemService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddItemRequest model, int collectionId)
    {
        var user = HttpContext.Items["User"] as User;
        var owns = await _collectionService.Owns(collectionId, user!);

        if (!owns)
        {
            return Unauthorized();
        }

        await _itemService.Create(model, collectionId);

        return Ok("Item created successfully");
    }

    [HttpPut]
    public async Task<IActionResult> Edit(EditItemRequest model, int collectionId)
    {
        var user = HttpContext.Items["User"] as User;
        var owns = await _collectionService.Owns(collectionId, user!);

        if (!owns)
        {
            return Unauthorized();
        }

        await _itemService.Edit(model, collectionId);

        return Ok("Item edited successfully");
    }

    [HttpDelete("{itemId:int}")]
    public async Task<IActionResult> Delete(int itemId, int collectionId)
    {
        var user = HttpContext.Items["User"] as User;
        var owns = await _collectionService.Owns(collectionId, user!);

        if (!owns)
        {
            return Unauthorized();
        }

        await _itemService.Delete(itemId);

        return Ok("Item deleted successfully");
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(IEnumerable<int> ids, int collectionId)
    {
        var user = HttpContext.Items["User"] as User;
        var owns = await _collectionService.Owns(collectionId, user!);

        if (!owns)
        {
            return Unauthorized();
        }

        await _itemService.Delete(ids);

        return Ok("Items deleted successfully");
    }

    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> Search(string searchString, int page, int count)
    {
        var items = await _itemService.Search(searchString, page, count);

        return Ok(items);
    }
}
