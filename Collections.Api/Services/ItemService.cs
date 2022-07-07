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

    Task<SearchItemsResponse> Search(string searchString, int page, int count);

    Task<CreateCommentResponse> CreateComment(CreateCommentRequest model, int itemId, int authorId);

    Task<GetCommentsResponse> GetComments(int itemId);

    Task Like(Item item, User author);

    Task Unlike(int itemId, int authorId);

    Task<GetItemResponse> Get(int itemId, int? userId);

    Task<GetLatestItemsResponse> GetLatestItems(int count);

    Task<GetTagsResponse> GetMostUsedTags(int count);

    Task<GetTagsResponse> SearchTags(string str, int count);

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
        item.Collection = collection;
        item.IntValues = _mapper.Map<List<IntValue>>(model.IntFields);
        item.BoolValues = _mapper.Map<List<BoolValue>>(model.BoolFields);
        item.StringValues = _mapper.Map<List<StringValue>>(model.StringFields);
        item.DateTimeValues = _mapper.Map<List<DateTimeValue>>(model.DateTimeFields);
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task Edit(EditItemRequest model, int itemId)
    {
        var item = await GetById(itemId);
        if (item is null)
        {
            throw new NotFoundException("Item not found");
        }

        if (!ValidateFields(model, await _collectionService.GetCollectionFields(item.Collection.Id)))
        {
            throw new BadHttpRequestException("Item's custom fields structure doesn't match collection");
        }

        var itemToUpdate = _mapper.Map<Item>(model);
        itemToUpdate.IntValues = _mapper.Map<List<IntValue>>(model.IntFields);
        itemToUpdate.BoolValues = _mapper.Map<List<BoolValue>>(model.BoolFields);
        itemToUpdate.StringValues = _mapper.Map<List<StringValue>>(model.StringFields);
        itemToUpdate.DateTimeValues = _mapper.Map<List<DateTimeValue>>(model.StringFields);
        _context.Items.Update(itemToUpdate);
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
        var item = await _context.Items.Include(i => i.Collection.Id)
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
            .FirstOrDefaultAsync(c => c.Id == item.Collection.Id))!.Fields;
        var like = await _context.Likes.FirstOrDefaultAsync(l => l.Author.Id == userId && l.Item.Id == itemId);
        var likesCount = await _context.Likes.CountAsync();
        var response = _mapper.Map<GetItemResponse>(item);
        response.Fields = _mapper.Map<List<FieldData>>(fields);
        response.Like = like is not null;
        response.LikesCount = likesCount;
        return response;
    }

    public async Task<GetLatestItemsResponse> GetLatestItems(int count)
    {
        var items = await _context.Items.Include(i => i.Collection)
            .ThenInclude(c => c.Owner)
            .OrderByDescending(i => i.Id)
            .Take(count)
            .ToListAsync();

        return new GetLatestItemsResponse { Items = _mapper.Map<List<LatestItemData>>(items) };
    }

    public async Task<GetTagsResponse> GetMostUsedTags(int count)
    {
        var tags = await _context.Tags
            .GroupBy(t => t.Name)
            .Select(n => new { Name = n.Key, Count = n.Count() })
            .OrderByDescending(n => n.Count)
            .Select(n => n.Name)
            .Take(count)
            .ToListAsync();
        return new GetTagsResponse { Tags = tags };
    }

    public async Task<GetTagsResponse> SearchTags(string str, int count)
    {
        var tags = await _context.Tags.Select(t=> t.Name).Distinct()
            .Where(t => t.Contains(str))
            .Take(count)
            .ToListAsync();
        return new GetTagsResponse { Tags = tags };
    }

    public async Task Delete(IEnumerable<int> ids)
    {
        _context.Items.RemoveRange(ids.Select(i => new Item { Id = i }));
        await _context.SaveChangesAsync();
    }

    public async Task<CreateCommentResponse> CreateComment(CreateCommentRequest model, int itemId, int authorId)
    {
        var comment = _mapper.Map<Comment>(model);
        comment.Item.Id = itemId;
        comment.Author.Id = authorId;
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return _mapper.Map<CreateCommentResponse>(comment);
    }

    public async Task<GetCommentsResponse> GetComments(int itemId)
    {
        return new GetCommentsResponse
        {
            Comments = _mapper.Map<List<CommentData>>(await _context.Comments.Where(c => c.Item.Id == itemId)
                .ToListAsync())
        };
    }

    public async Task Like(Item item, User author)
    {
        await _context.Likes.AddAsync(new Like { Author = author, Item = item });
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

    public async Task<SearchItemsResponse> Search(string searchString, int page, int count)
    {
        var pageCount = (int)Math.Ceiling(await _context.Items.CountAsync() / (double)count);
        var items = await _context.Items.Where(i =>
                i.SimpleSearchVector.Matches(searchString) ||
                i.StringValues.Any(s => s.SimpleSearchVector.Matches(searchString)) ||
                i.Comments.Any(s => s.SimpleSearchVector.Matches(searchString)))
            .Skip((page - 1) * count)
            .Take(count)
            .ToListAsync();
        return new SearchItemsResponse { PagesCount = pageCount, Items = _mapper.Map<List<SearchItemData>>(items) };
    }

    public async Task<Item?> GetById(int id)
    {
        return await _context.Items.FindAsync(id);
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
        return firstList.Count == secondList.Count &&
               firstList.TrueForAll(f => secondList.Exists(s => f.FieldId == s.Id));
    }
}
