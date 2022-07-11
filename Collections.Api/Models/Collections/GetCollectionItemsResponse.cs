namespace Collections.Api.Models.Collections;

public class GetCollectionItemsResponse
{
    public bool IsOwner { get; set; }
    
    public List<CollectionItemData> Items { get; set; }
}
