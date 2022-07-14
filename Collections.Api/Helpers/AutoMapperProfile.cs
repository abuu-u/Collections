using AutoMapper;
using Collections.Api.Entities;
using Collections.Api.Models.Collections;
using Collections.Api.Models.Users;

namespace Collections.Api.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, AuthenticationResponse>();

        CreateMap<User, UserData>();

        CreateMap<RegisterRequest, User>();

        CreateMap<FieldData, Field>();

        CreateMap<Field, FieldData>();

        CreateMap<EditCollectionFieldData, Field>();
        
        CreateMap<EditCollectionFieldData, FieldData>();

        CreateMap<EditCollectionRequest, Collection>();

        CreateMap<EditCollectionRequest, CollectionData>();

        CreateMap<Collection, CollectionData>();

        CreateMap<CollectionData, Collection>();

        CreateMap<CreateFieldData, Field>();

        CreateMap<CreateCollectionRequest, Collection>();

        CreateMap<CreateCollectionRequest, CollectionData>();

        CreateMap<IntValueData, IntValue>();

        CreateMap<StringValueData, StringValue>();

        CreateMap<BoolValueData, BoolValue>();

        CreateMap<DateTimeValueData, DateTimeValue>();

        CreateMap<IntValue, IntValueData>();

        CreateMap<StringValue, StringValueData>();

        CreateMap<BoolValue, BoolValueData>();

        CreateMap<DateTimeValue, DateTimeValueData>();

        CreateMap<string, Tag>().ConvertUsing(n => new Tag { Name = n });

        CreateMap<Tag, string>().ConvertUsing(t => t.Name);

        CreateMap<Topic, TopicData>();

        CreateMap<AddItemRequest, Item>();

        CreateMap<EditItemRequest, Item>();

        CreateMap<Item, ItemData>();

        CreateMap<CreateCommentRequest, Comment>();

        CreateMap<Comment, CreateCommentResponse>();

        CreateMap<User, CommentAuthorData>();

        CreateMap<Comment, CommentData>();

        CreateMap<Item, SearchItemData>();

        CreateMap<Item, GetItemResponse>();

        CreateMap<Item, GetItemForEditingResponse>();

        CreateMap<Item, LatestItemData>();

        CreateMap<Collection, SearchCollectionData>();

        CreateMap<Collection, LatestCollectionData>();

        CreateMap<Collection, GetCollectionResponse>();

        CreateMap<Collection, GetCollectionForEditResponse>();

        CreateMap<User, LatestItemOwnerData>();
    }
}
