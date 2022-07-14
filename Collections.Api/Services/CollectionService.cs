using System.Text.Json;
using AutoMapper;
using Collections.Api.Entities;
using Collections.Api.Helpers;
using Collections.Api.Models.Collections;
using Microsoft.EntityFrameworkCore;

namespace Collections.Api.Services;

public interface ICollectionService
{
    Task Create(CreateCollectionRequest model, User owner);

    Task<CollectionData> Edit(EditCollectionRequest model);

    Task Delete(int collectionId);

    Task<GetAllCollectionResponse> GetMyCollections(int page, int count, int ownerId);

    Task<Collection?> GetById(int id);

    Task<bool> Owns(int collectionId, User user);

    Task<List<Field>> GetCollectionFields(int collectionId);

    Task<GetFieldsResponse> GetFields(int collectionId);

    Task<List<TopicData>> GetTopics();

    Task<GetCollectionItemsResponse> GetItems(GetCollectionItemsRequest model, int collectionId, bool isOwner);

    Task<SearchCollectionsResponse> Search(string? searchString, int page, int count);

    Task<GetTagsResponse> GetCollectionTags(int collectionId);

    Task<GetLargestCollectionsResponse> GetLargestCollections(int count);

    Task<GetCollectionResponse> Get(int id, bool isOwner);

    Task<GetCollectionForEditResponse> GetForEdit(int id);
}

public class CollectionService : ICollectionService
{
    private readonly DataContext _context;

    private readonly IMapper _mapper;

    public CollectionService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Create(CreateCollectionRequest model, User owner)
    {
        if (model.Fields.Count == 0)
        {
            throw new BadHttpRequestException("Collection must have fields");
        }
        if (model.Fields.DistinctBy(f => f.Name).Count() != model.Fields.Count)
        {
            throw new BadHttpRequestException("Field names should be unique");
        }
        var collectionToAdd = _mapper.Map<Collection>(model);
        collectionToAdd.Owner = owner;
        await _context.Collections.AddAsync(collectionToAdd);
        await _context.SaveChangesAsync();
    }

    public async Task<CollectionData> Edit(EditCollectionRequest model)
    {
        if (model.Fields.Count == 0)
        {
            throw new BadHttpRequestException("Collection must have fields");
        }
        var collection = await GetById(model.Id);
        if (collection is null)
        {
            throw new NotFoundException("Collection not found");
        }
        var collectionToUpdate = _mapper.Map<Collection>(model);
        collectionToUpdate.OwnerId = collection.OwnerId;
        _context.Collections.Update(collectionToUpdate);
        await _context.SaveChangesAsync();
        return _mapper.Map<CollectionData>(model);
    }

    public async Task Delete(int collectionId)
    {
        var collection = await GetById(collectionId);
        if (collection is null)
        {
            throw new NotFoundException("Collection not found");
        }
        _context.Collections.Remove(collection);
        await _context.SaveChangesAsync();
    }

    public async Task<GetAllCollectionResponse> GetMyCollections(int page, int count, int ownerId)
    {
        var pageCount = (int)Math.Ceiling(await _context.Collections.Where(c => c.Owner.Id == ownerId).CountAsync() /
                                          (double)count);
        var collections = await _context.Collections.Where(c => c.Owner.Id == ownerId)
            .Skip((page - 1) * count)
            .Take(count)
            .ToListAsync();
        return new GetAllCollectionResponse
        {
            PagesCount = pageCount, Collections = _mapper.Map<List<CollectionData>>(collections)
        };
    }

    public async Task<GetFieldsResponse> GetFields(int collectionId)
    {
        var collection = await GetById(collectionId);
        if (collection is null)
        {
            throw new NotFoundException("Collection couldn't be found");
        }
        var fields = await _context.Fields.Where(f => f.Collection.Id == collectionId).ToListAsync();
        return new GetFieldsResponse
        {
            Fields = _mapper.Map<List<FieldData>>(fields)
        };
    }

    public async Task<List<TopicData>> GetTopics()
    {
        var topics = await _context.Topics.ToListAsync();
        return _mapper.Map<List<TopicData>>(topics);
    }

    public async Task<GetCollectionItemsResponse> GetItems(GetCollectionItemsRequest model,
        int collectionId,
        bool isOwner)
    {
        var itemsQuery = _context.Items.Include(i => i.StringValues)
            .Include(i => i.DateTimeValues)
            .Where(i => i.Collection.Id == collectionId);
        if (model.FilterName is not null)
        {
            itemsQuery = itemsQuery.Where(i => i.Name.Contains(model.FilterName));
        }
        if (model.FilterTags is not null)
        {
            itemsQuery = itemsQuery.Where(i => i.Tags.Any(t => model.FilterTags.Contains(t.Name)));
        }
        if (model.SortFieldId is not null)
        {
            if (model.SortBy is not ("desc" or "asc" or null))
            {
                throw new BadHttpRequestException("SortBy value should be \"asc\" or \"desc\" or empty");
            }
            var field = await _context.Fields.FirstOrDefaultAsync(f =>
                f.CollectionId == collectionId && f.Id == model.SortFieldId &&
                (f.FieldType == FieldType.String || f.FieldType == FieldType.DateTime));
            if (field is null && model.SortFieldId > 0)
            {
                throw new NotFoundException("Field not found");
            }
            itemsQuery = model.SortBy switch
            {
                "desc" => model.SortFieldId < 0
                    ? model.SortFieldId switch
                    {
                        (int)FixedFields.Name => itemsQuery.OrderByDescending(i => i.Name),
                        _ => itemsQuery
                    }
                    : field?.FieldType switch
                    {
                        FieldType.DateTime => itemsQuery.OrderByDescending(i =>
                            i.DateTimeValues.First(v => v.FieldId == model.SortFieldId).Value),
                        FieldType.String or FieldType.MultiLineString => itemsQuery.OrderByDescending(i =>
                            i.StringValues.First(v => v.FieldId == model.SortFieldId).Value),
                        _ => itemsQuery
                    },
                _ => model.SortFieldId < 0
                    ? model.SortFieldId switch
                    {
                        (int)FixedFields.Name => itemsQuery.OrderBy(i => i.Name),
                        _ => itemsQuery
                    }
                    : field?.FieldType switch
                    {
                        FieldType.DateTime => itemsQuery.OrderBy(i =>
                            i.DateTimeValues.First(v => v.FieldId == model.SortFieldId).Value),
                        FieldType.String or FieldType.MultiLineString => itemsQuery.OrderBy(i =>
                            i.StringValues.First(v => v.FieldId == model.SortFieldId).Value),
                        _ => itemsQuery
                    }
            };
        }
        var items = await itemsQuery.ToListAsync();
        return new GetCollectionItemsResponse
        {
            IsOwner = isOwner, Items = _mapper.Map<List<ItemData>>(items)
        };
    }

    public async Task<GetLargestCollectionsResponse> GetLargestCollections(int count)
    {
        var collections = await _context.Collections.OrderByDescending(c => c.Items.Count).Take(count).ToListAsync();
        return new GetLargestCollectionsResponse
        {
            Collections = _mapper.Map<List<CollectionData>>(collections)
        };
    }

    public async Task<GetCollectionResponse> Get(int id, bool isOwner)
    {
        var collection = await _context.Collections.Include(c => c.Fields)
            .Include(c => c.Items)
            .ThenInclude(i => i.StringValues)
            .Include(c => c.Items)
            .ThenInclude(i => i.DateTimeValues)
            .Include(c => c.Items)
            .ThenInclude(i => i.IntValues)
            .Include(c => c.Items)
            .ThenInclude(i => i.BoolValues)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (collection is null)
        {
            throw new NotFoundException("User collection with this id not found");
        }
        var response = _mapper.Map<GetCollectionResponse>(collection);
        response.IsOwner = isOwner;
        return response;
    }

    public async Task<GetCollectionForEditResponse> GetForEdit(int id)
    {
        var collection = await _context.Collections.Include(c => c.Fields)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (collection is null)
        {
            throw new NotFoundException("User collection with this id not found");
        }
        return _mapper.Map<GetCollectionForEditResponse>(collection);
    }

    public async Task<SearchCollectionsResponse> Search(string? searchString, int page, int count)
    {
        var pageCount = (int)Math.Ceiling(await _context.Collections.CountAsync() / (double)count);
        var collectionsQuery = _context.Collections
            .Skip((page - 1) * count)
            .Take(count);
        if (searchString is not null)
        {
            collectionsQuery = collectionsQuery.Where(i =>
                i.SimpleSearchVector.Matches(EF.Functions.PlainToTsQuery("simple", searchString)) ||
                i.Fields.Any(f => f.SimpleSearchVector.Matches(EF.Functions.PlainToTsQuery("simple", searchString))) ||
                i.Items.Any(i =>
                    i.SimpleSearchVector.Matches(EF.Functions.PlainToTsQuery("simple", searchString)) ||
                    i.StringValues.Any(s => s.SimpleSearchVector.Matches(EF.Functions.PlainToTsQuery("simple", searchString))) ||
                    i.Tags.Any(t => t.SimpleSearchVector.Matches(EF.Functions.PlainToTsQuery("simple", searchString))) ||
                    i.Comments.Any(c => c.SimpleSearchVector.Matches(EF.Functions.PlainToTsQuery("simple", searchString)))));
        }
        var collections = await collectionsQuery.ToListAsync();
        return new SearchCollectionsResponse
        {
            PagesCount = pageCount, Collections = _mapper.Map<List<SearchCollectionData>>(collections)
        };
    }

    public async Task<GetTagsResponse> GetCollectionTags(int collectionId)
    {
        var tags = await _context.Tags.Where(t => t.Item.Collection.Id == collectionId)
            .DistinctBy(t => t.Name)
            .ToListAsync();
        return new GetTagsResponse
        {
            Tags = _mapper.Map<List<string>>(tags)
        };
    }

    public async Task<Collection?> GetById(int id)
    {
        return await _context.Collections.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> Owns(int collectionId, User user)
    {
        return user.Admin ||
               (await _context.Collections.FirstOrDefaultAsync(c => c.Owner.Id == user.Id && c.Id == collectionId)) is
               not null;
    }

    public async Task<List<Field>> GetCollectionFields(int collectionId)
    {
        return await _context.Fields.Where(f => f.Collection.Id == collectionId).ToListAsync();
    }
}
