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

        CreateMap<CreateFieldData, Field>();

        CreateMap<CreateCollectionRequest, Collection>();

        CreateMap<CreateCollectionRequest, CollectionData>();

        CreateMap<IntValueData, IntValue>();

        CreateMap<StringValueData, StringValue>();

        CreateMap<BoolValueData, BoolValue>();

        CreateMap<DateTimeValueData, DateTimeValue>();

        CreateMap<string, Tag>().ConvertUsing(n => new Tag { Name = n });

        CreateMap<Tag, string>().ConvertUsing(t => t.Name);

        CreateMap<Topic, TopicData>();

        CreateMap<AddItemRequest, Item>();

        CreateMap<Item, CollectionItemData>();

        CreateMap<CreateCommentRequest, Comment>();

        CreateMap<User, CommentAuthorData>();

        CreateMap<Comment, CommentData>();

        CreateMap<Item, SearchItemData>();

        CreateMap<Item, GetItemResponse>();

        CreateMap<Item, LatestItemData>();

        CreateMap<Collection, SearchCollectionData>();

        CreateMap<Collection, LatestCollectionData>();

        CreateMap<User, LatestItemOwnerData>();
    }
}
