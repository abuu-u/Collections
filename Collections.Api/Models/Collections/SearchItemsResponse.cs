namespace Collections.Api.Models.Collections;

public class SearchResponse
{
    public List<SearchItemData> Items { get; set; }

    public List<SearchCollectionData> Collections { get; set; }
}
