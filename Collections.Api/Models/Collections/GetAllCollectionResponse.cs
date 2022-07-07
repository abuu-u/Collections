using Collections.Api.Entities;

namespace Collections.Api.Models.Collections;

public class GetAllCollectionResponse
{
    public int PagesCount { get; set; }

    public List<CollectionData> Collections { get; set; }
}
