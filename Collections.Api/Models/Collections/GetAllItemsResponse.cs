namespace Collections.Api.Models.Collections;

public class GetAllItemsResponse
{
    public int PagesCount { get; set; }

    public List<CollectionData> Collections { get; set; }
}
