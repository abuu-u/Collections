namespace Collections.Api.Models.Collections;

public class SearchCollectionsResponse
{
    public int PagesCount { get; set; }

    public List<SearchCollectionData> Collections { get; set; }
}
