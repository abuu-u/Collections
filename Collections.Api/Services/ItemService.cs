using System.Text.Json;
using AutoMapper;
using Collections.Api.Entities;
using Collections.Api.Helpers;
using Collections.Api.Models.Collections;
using Microsoft.EntityFrameworkCore;

namespace Collections.Api.Services;

public interface IItemService
{
    Task Create(AddItemRequest model, int collectionId);

    Task Edit(EditItemRequest model, int itemId);

    Task Delete(int itemId);

    Task Delete(IEnumerable<int> ids);

    Task<SearchItemsResponse> Search(string? searchString, int page, int count);

    Task<CreateCommentResponse> CreateComment(CreateCommentRequest model, int itemId, User author);

    Task<GetCommentsResponse> GetComments(int itemId);

    Task Like(int itemId, User author);

    Task Unlike(int itemId, int authorId);

    Task<GetItemResponse> Get(int itemId, int? userId);

    Task<GetItemForEditingResponse> GetItemForEditing(int itemId, int? userId);

    Task<GetLatestItemsResponse> GetLatestItems(int count);

    Task<GetTagsResponse> GetMostUsedTags(int count);

    Task<GetTagsResponse> SearchTags(string? str, int count);

    Task<Item?> GetById(int id);
}

public class ItemService : IItemService
{
    private readonly ICollectionService _collectionService;

    private readonly DataContext _context;

    private readonly IMapper _mapper;

    public ItemService(ICollectionService collectionService, DataContext context, IMapper mapper)
    {
        _collectionService = collectionService;
        _context = context;
        _mapper = mapper;
    }

    public async Task Create(AddItemRequest model, int collectionId)
    {
        var collection = await _collectionService.GetById(collectionId);
        if (collection is null)
        {
            throw new NotFoundException("Collection not found");
        }
        if (!ValidateFields(model, (await _collectionService.GetCollectionFields(collectionId))))
        {
            throw new BadHttpRequestException("Item's custom fields structure doesn't match collection");
        }

        var item = _mapper.Map<Item>(model);
        item.CollectionId = collection.Id;
        item.IntValues = _mapper.Map<List<IntValue>>(model.IntFields);
        item.BoolValues = _mapper.Map<List<BoolValue>>(model.BoolFields);
        item.StringValues = _mapper.Map<List<StringValue>>(model.StringFields);
        item.DateTimeValues = _mapper.Map<List<DateTimeValue>>(model.DateTimeFields);
        await _context.Items.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task Edit(EditItemRequest model, int collectionId)
    {
        var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == model.Id);
        if (item is null)
        {
            throw new NotFoundException("Item not found");
        }

        if (!ValidateFields(model, await _collectionService.GetCollectionFields(collectionId)))
        {
            throw new BadHttpRequestException("Item's custom fields structure doesn't match collection");
        }
        item = _mapper.Map<Item>(model);
        item.CollectionId = collectionId;
        _context.IntValues.UpdateRange(_mapper.Map<List<IntValue>>(model.IntFields));
        _context.BoolValues.UpdateRange(_mapper.Map<List<BoolValue>>(model.BoolFields));
        _context.StringValues.UpdateRange(_mapper.Map<List<StringValue>>(model.StringFields));
        _context.DateTimeValues.UpdateRange(_mapper.Map<List<DateTimeValue>>(model.DateTimeFields));
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int itemId)
    {
        var item = await GetById(itemId);
        if (item is null)
        {
            throw new NotFoundException("Item not found");
        }

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task<GetItemResponse> Get(int itemId, int? userId)
    {
        var item = await _context.Items
            .Include(i => i.Tags)
            .Include(i => i.BoolValues)
            .Include(i => i.IntValues)
            .Include(i => i.StringValues)
            .Include(i => i.DateTimeValues)
            .Include(i => i.Comments.OrderBy(c => c.Id))
            .ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(i => i.Id == itemId);
        if (item is null)
        {
            throw new NotFoundException("Item not found");
        }
        var fields = await _collectionService.GetFields(item.CollectionId);
        var like = await _context.Likes.FirstOrDefaultAsync(l => l.Author.Id == userId && l.Item.Id == itemId);
        var likesCount = await _context.Likes.Where(l => l.Item.Id == itemId).CountAsync();
        var response = _mapper.Map<GetItemResponse>(item);
        response.Fields = _mapper.Map<List<FieldData>>(fields.Fields);
        response.Like = like is not null;
        response.LikesCount = likesCount;
        return response;
    }

    public async Task<GetItemForEditingResponse> GetItemForEditing(int itemId, int? userId)
    {
        var item = await _context.Items
            .Include(i => i.Tags)
            .Include(i => i.BoolValues)
            .Include(i => i.IntValues)
            .Include(i => i.StringValues)
            .Include(i => i.DateTimeValues)
            .FirstOrDefaultAsync(i => i.Id == itemId);
        if (item is null)
        {
            throw new NotFoundException("Item not found");
        }
        var fields = (await _context.Collections.Include(c => c.Fields)
            .FirstOrDefaultAsync(c => c.Id == item.CollectionId))!.Fields;
        var response = _mapper.Map<GetItemForEditingResponse>(item);
        response.BoolFields = _mapper.Map<List<BoolValueData>>(item.BoolValues);
        response.IntFields = _mapper.Map<List<IntValueData>>(item.IntValues);
        response.StringFields = _mapper.Map<List<StringValueData>>(item.StringValues);
        response.DateTimeFields = _mapper.Map<List<DateTimeValueData>>(item.DateTimeValues);
        response.Fields = _mapper.Map<List<FieldData>>(fields);
        return response;
    }

    public async Task<GetLatestItemsResponse> GetLatestItems(int count)
    {
        var items = await _context.Items.Include(i => i.Collection.Owner)
            .OrderByDescending(i => i.Id)
            .AsSplitQuery()
            .Take(count)
            .ToListAsync();
        return new GetLatestItemsResponse
        {
            Items = _mapper.Map<List<LatestItemData>>(items)
        };
    }

    public async Task<GetTagsResponse> GetMostUsedTags(int count)
    {
        var tags = await _context.Tags
            .GroupBy(t => t.Name)
            .Select(n => new
            {
                Name = n.Key, Count = n.Count()
            })
            .OrderByDescending(n => n.Count)
            .Select(n => n.Name)
            .Take(count)
            .ToListAsync();
        return new GetTagsResponse
        {
            Tags = tags
        };
    }

    public async Task<GetTagsResponse> SearchTags(string? str, int count)
    {
        var tags = await _context.Tags.Select(t => t.Name)
            .Distinct()
            .Where(t => t.Contains(str ?? ""))
            .Take(count)
            .ToListAsync();
        return new GetTagsResponse
        {
            Tags = tags
        };
    }

    public async Task Delete(IEnumerable<int> ids)
    {
        _context.Items.RemoveRange(ids.Select(i => new Item
        {
            Id = i
        }));
        await _context.SaveChangesAsync();
    }

    public async Task<CreateCommentResponse> CreateComment(CreateCommentRequest model, int itemId, User author)
    {
        var item = await GetById(itemId);
        if (item is null)
        {
            throw new NotFoundException("Item not found");
        }
        var comment = _mapper.Map<Comment>(model);
        comment.ItemId = item.Id;
        comment.AuthorId = author.Id;
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return new CreateCommentResponse
        {
            Comment = _mapper.Map<CommentData>(comment)
        };
    }

    public async Task<GetCommentsResponse> GetComments(int itemId)
    {
        var comments = await _context.Comments.Include(c => c.Author)
            .Where(c => c.Item.Id == itemId)
            .OrderBy(c => c.Id)
            .ToListAsync();
        return new GetCommentsResponse
        {
            Comments = _mapper.Map<List<CommentData>>(comments)
        };
    }

    public async Task Like(int itemId, User author)
    {
        var item = await GetById(itemId);
        if (item is null)
        {
            throw new NotFoundException("Item not found");
        }
        await _context.Likes.AddAsync(new Like
        {
            AuthorId = author.Id, ItemId = item.Id
        });
        await _context.SaveChangesAsync();
    }

    public async Task Unlike(int itemId, int authorId)
    {
        var like = await _context.Likes.FirstOrDefaultAsync(l => l.Author.Id == authorId && l.Item.Id == itemId);
        if (like is null)
        {
            throw new NotFoundException("Like not found");
        }

        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();
    }

    public async Task<SearchItemsResponse> Search(string? searchString, int page, int count)
    {
        var pageCount = (int)Math.Ceiling(await _context.Items.CountAsync() / (double)count);
        var itemsQuery = _context.Items
            .Skip((page - 1) * count)
            .Take(count);
        if (searchString is not null)
        {
            itemsQuery = itemsQuery.Where(i =>
                i.SimpleSearchVector.Matches(EF.Functions.PlainToTsQuery("simple", searchString)) ||
                i.StringValues.Any(s =>
                    s.SimpleSearchVector.Matches(EF.Functions.PlainToTsQuery("simple", searchString))) ||
                i.Tags.Any(s => s.SimpleSearchVector.Matches(EF.Functions.PlainToTsQuery("simple", searchString))) ||
                i.Collection.SimpleSearchVector.Matches(EF.Functions.PlainToTsQuery("simple", searchString)) ||
                i.Collection.Fields.Any(f => f.SimpleSearchVector.Matches(EF.Functions.PlainToTsQuery("simple", searchString))) ||
                i.Comments.Any(c => c.SimpleSearchVector.Matches(EF.Functions.PlainToTsQuery("simple", searchString))));
        }
        var items = await itemsQuery.ToListAsync();
        return new SearchItemsResponse
        {
            PagesCount = pageCount, Items = _mapper.Map<List<SearchItemData>>(items)
        };
    }

    public async Task<Item?> GetById(int id)
    {
        return await _context.Items.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
    }

    private static bool ValidateFields<T>(T item, List<Field> collectionFields) where T : IHasFields
    {
        return collectionFields.Count == item.BoolFields.Count + item.IntFields.Count + item.StringFields.Count +
               item.DateTimeFields.Count && CheckFieldsEquality(item.IntFields, collectionFields) &&
               CheckFieldsEquality(item.BoolFields, collectionFields) &&
               CheckFieldsEquality(item.StringFields, collectionFields) &&
               CheckFieldsEquality(item.DateTimeFields, collectionFields);
    }

    private static bool CheckFieldsEquality<T1, T2>(List<T1> firstList, List<T2> secondList)
        where T1 : IHasFieldId where T2 : IHasId
    {
        return firstList.TrueForAll(f => secondList.Exists(s => f.FieldId == s.Id));
    }
}
