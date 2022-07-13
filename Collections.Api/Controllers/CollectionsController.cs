using System.Text.Json;
using Collections.Api.Authorization;
using Collections.Api.Entities;
using Collections.Api.Models.Collections;
using Collections.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Collections.Api.Controllers;

[Authorize]
[ApiController]
[Route("collections/[controller]")]
public class CollectionsController : ControllerBase
{
    private readonly ICollectionService _collectionService;

    public CollectionsController(ICollectionService collectionService)
    {
        _collectionService = collectionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyCollections(int page, int count)
    {
        var user = HttpContext.Items["User"] as User;
        var response = await _collectionService.GetMyCollections(page, count, user!.Id);
        return Ok(response);
    }

    [HttpGet("{collectionId:int}")]
    public async Task<IActionResult> Get(int collectionId)
    {
        var user = HttpContext.Items["User"] as User;
        var collection = await _collectionService.GetById(collectionId);
        if (collection is null)
        {
            return NotFound(new { message = "User collection with this id not found" });
        }
        var owns = await _collectionService.Owns(collection.Id, user!);
        if (!owns)
        {
            return NotFound(new { message = "User collection with this id not found" });
        }
        var response = await _collectionService.Get(collectionId);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCollectionRequest model)
    {
        var user = HttpContext.Items["User"] as User;
        await _collectionService.Create(model, user!);
        return Ok(new { message = "Collection created successfully" });
    }

    [HttpPut]
    public async Task<IActionResult> Edit(EditCollectionRequest model)
    {
        var user = HttpContext.Items["User"] as User;
        var owns = await _collectionService.Owns(model.Id, user!);
        if (!owns)
        {
            return Unauthorized(new { message = "Unauthorized" });
        }
        await _collectionService.Edit(model);
        return Ok(new { message = "Collection edited successfully" });
    }

    [HttpDelete("{collectionId:int}")]
    public async Task<IActionResult> Delete(int collectionId)
    {
        var user = HttpContext.Items["User"] as User;
        var owns = await _collectionService.Owns(collectionId, user!);
        if (!owns)
        {
            return Unauthorized(new { message = "Unauthorized" });
        }
        await _collectionService.Delete(collectionId);
        return Ok(new { message = "Collection deleted successfully" });
    }

    [AllowAnonymous]
    [HttpGet("{collectionId:int}/items")]
    public async Task<IActionResult> GetItems([FromQuery] GetCollectionItemsRequest model, int collectionId)
    {
        var owns = HttpContext.Items["User"] is User user && await _collectionService.Owns(collectionId, user);
        var items = await _collectionService.GetItems(model, collectionId, owns);
        return Ok(items);
    }

    [AllowAnonymous]
    [HttpGet("{collectionId:int}/fields")]
    public async Task<IActionResult> GetFields(int collectionId)
    {
        var fields = await _collectionService.GetFields(collectionId);
        return Ok(fields);
    }

    [AllowAnonymous]
    [HttpGet("topics")]
    public async Task<IActionResult> DownloadTopics()
    {
        var topics = await _collectionService.GetTopics();
        var json = System.Text.Json.JsonSerializer.Serialize(topics);
        var byteArray = System.Text.Encoding.ASCII.GetBytes(json);
        return File(byteArray, "application/force-download", "topics.json");
    }

    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> Search(string? searchString, int page, int count)
    {
        var collections = await _collectionService.Search(searchString, page, count);
        return Ok(collections);
    }

    [AllowAnonymous]
    [HttpGet("largest")]
    public async Task<IActionResult> GetLargestCollections(int count)
    {
        var collections = await _collectionService.GetLargestCollections(count);
        return Ok(collections);
    }

    [AllowAnonymous]
    [HttpGet("{collectionId:int}/tags")]
    public async Task<IActionResult> GetCollectionTags(int collectionId)
    {
        var tags = await _collectionService.GetCollectionTags(collectionId);
        return Ok(tags);
    }
}
