namespace Collections.Api.Models.Collections;

public class SearchItemsResponse
{
    public int PagesCount { get; set; }

    public List<SearchItemData> Items { get; set; }
}
