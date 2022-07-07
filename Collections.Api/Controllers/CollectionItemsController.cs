using Collections.Api.Authorization;
using Collections.Api.Entities;
using Collections.Api.Models.Collections;
using Collections.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Collections.Api.Controllers;

[Authorize]
[ApiController]
[Route("CollectionsApi/Collections/{collectionId:int}/Items")]
public class CollectionItemsController : ControllerBase
{
    private readonly ICollectionService _collectionService;

    private readonly IItemService _itemService;

    public CollectionItemsController(ICollectionService collectionService, IItemService itemService)
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
            return Unauthorized(new { message = "Unauthorized"});
        }
        await _itemService.Create(model, collectionId);
        return Ok(new { message = "Item created successfully"});
    }

    [HttpPut]
    public async Task<IActionResult> Edit(EditItemRequest model, int collectionId)
    {
        var user = HttpContext.Items["User"] as User;
        var owns = await _collectionService.Owns(collectionId, user!);
        if (!owns)
        {
            return Unauthorized(new { message = "Unauthorized"});
        }
        await _itemService.Edit(model, collectionId);
        return Ok(new { message = "Item edited successfully"});
    }

    [HttpDelete("{itemId:int}")]
    public async Task<IActionResult> Delete(int itemId, int collectionId)
    {
        var user = HttpContext.Items["User"] as User;
        var owns = await _collectionService.Owns(collectionId, user!);
        if (!owns)
        {
            return Unauthorized(new { message = "Unauthorized"});
        }
        await _itemService.Delete(itemId);
        return Ok(new { message = "Item deleted successfully"});
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(IEnumerable<int> ids, int collectionId)
    {
        var user = HttpContext.Items["User"] as User;
        var owns = await _collectionService.Owns(collectionId, user!);
        if (!owns)
        {
            return Unauthorized(new { message = "Unauthorized"});
        }
        await _itemService.Delete(ids);
        return Ok(new { message ="Items deleted successfully"});
    }
}
